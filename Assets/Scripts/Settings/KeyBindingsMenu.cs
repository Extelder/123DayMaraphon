using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// Вкладка "Управление" в настройках. Собирается в рантайме из уже существующих элементов меню
// (вкладка Color, панель GeneralSettings, строка вида "Blood Splat"), поэтому выглядит как родная
// и не требует правок префабов.
public class KeyBindingsMenu : MonoBehaviour
{
    private const string TableName = "KlitterTranslationTable";
    private const string GeneralPanelName = "GeneralSettings";
    private const string GlitchGlyphs = "#$%&*+/<>=?@_01ABCDEFGHJKLMNPQRSTUVWXYZ";
    private const float DecodeDuration = 0.28f;
    private const float GlitchFrame = 0.045f;

    private struct BindingInfo
    {
        public GameAction Action;
        public string Key;
        public string Fallback;

        public BindingInfo(GameAction action, string key, string fallback)
        {
            Action = action;
            Key = key;
            Fallback = fallback;
        }
    }

    private struct SectionInfo
    {
        public string Key;
        public string Fallback;
        public BindingInfo[] Bindings;

        public SectionInfo(string key, string fallback, params BindingInfo[] bindings)
        {
            Key = key;
            Fallback = fallback;
            Bindings = bindings;
        }
    }

    private class BindingRow
    {
        public GameAction Action;
        public TMP_Text KeyText;
        public KeyCode Shown = KeyCode.None;
        public Coroutine Effect;
    }

    private static readonly SectionInfo[] Sections =
    {
        new SectionInfo("bind_header_movement", "Movement",
            new BindingInfo(GameAction.MoveForward, "bind_move_forward", "Forward"),
            new BindingInfo(GameAction.MoveBack, "bind_move_back", "Back"),
            new BindingInfo(GameAction.MoveLeft, "bind_move_left", "Left"),
            new BindingInfo(GameAction.MoveRight, "bind_move_right", "Right"),
            new BindingInfo(GameAction.Jump, "bind_jump", "Jump"),
            new BindingInfo(GameAction.Dash, "bind_dash", "Dash"),
            new BindingInfo(GameAction.DashDown, "bind_dash_down", "Dash down")),
        new SectionInfo("bind_header_combat", "Combat",
            new BindingInfo(GameAction.Shoot, "bind_shoot", "Shoot"),
            new BindingInfo(GameAction.Ability, "bind_ability", "Alt fire"),
            new BindingInfo(GameAction.Katana, "bind_katana", "Katana"),
            new BindingInfo(GameAction.Ultimate, "bind_ultimate", "Ultimate")),
        new SectionInfo("bind_header_weapons", "Weapons",
            new BindingInfo(GameAction.Weapon1, "bind_weapon_1", "Shotgun"),
            new BindingInfo(GameAction.Weapon2, "bind_weapon_2", "Rifle / RPG"),
            new BindingInfo(GameAction.Weapon3, "bind_weapon_3", "Railgun")),
    };

    private static KeyCode[] _bindableKeys;

    private readonly List<BindingRow> _rows = new List<BindingRow>();
    private readonly List<Action> _unsubscribers = new List<Action>();

    private SettingsPanelChoose _switcher;
    private CanvasGroup _inputBlocker;
    private BindingRow _listening;
    private int _listenStartFrame;
    private KeyCode _waitForRelease = KeyCode.None;
    private string _pressKeyText = "Press key";

    private static KeyCode[] BindableKeys
    {
        get
        {
            if (_bindableKeys != null)
                return _bindableKeys;

            List<KeyCode> keys = new List<KeyCode>();
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (KeyBindings.CanBind(key) && !keys.Contains(key))
                    keys.Add(key);
            }

            _bindableKeys = keys.ToArray();
            return _bindableKeys;
        }
    }

    public static void Install(GameObject settingsCanvas)
    {
        if (settingsCanvas == null)
            return;

        SettingsPanelChoose switcher = settingsCanvas.GetComponentInChildren<SettingsPanelChoose>(true);
        if (switcher == null || switcher.GetComponent<KeyBindingsMenu>() != null)
            return;

        try
        {
            switcher.gameObject.AddComponent<KeyBindingsMenu>().Build(switcher, settingsCanvas.transform);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    private void Build(SettingsPanelChoose switcher, Transform canvasRoot)
    {
        _switcher = switcher;

        List<RectTransform> tabs = GetTabs(switcher.transform);
        Transform generalPanel = FindDeep(canvasRoot, GeneralPanelName);
        Transform rowTemplate = generalPanel != null ? FindRowTemplate(generalPanel) : null;
        if (tabs.Count == 0 || rowTemplate == null)
        {
            Debug.LogWarning("KeyBindingsMenu: settings layout is not recognized, controls tab is not created");
            return;
        }

        GameObject panel = BuildPanel(generalPanel, rowTemplate);
        BuildTab(tabs, panel);

        _inputBlocker = canvasRoot.GetComponent<CanvasGroup>();
        if (_inputBlocker == null)
            _inputBlocker = canvasRoot.gameObject.AddComponent<CanvasGroup>();

        LocalizeTo("bind_press_key", "Press key", value => _pressKeyText = value);
        KeyBindings.Changed += OnBindingsChanged;

        foreach (BindingRow row in _rows)
            ShowKey(row, false);
    }

    private void OnDisable()
    {
        StopListening(false);
        _waitForRelease = KeyCode.None;
        SetUiBlocked(false);

        foreach (BindingRow row in _rows)
            ShowKey(row, false);
    }

    private void OnDestroy()
    {
        KeyBindings.Changed -= OnBindingsChanged;
        if (_listening != null)
            KeyBindings.IsListening = false;

        foreach (Action unsubscribe in _unsubscribers)
            unsubscribe();
        _unsubscribers.Clear();
    }

    private void Update()
    {
        if (_waitForRelease != KeyCode.None && !Input.GetKey(_waitForRelease))
        {
            _waitForRelease = KeyCode.None;
            if (_listening == null)
                SetUiBlocked(false);
        }

        // В кадре клика по кнопке ещё не слушаем, иначе сразу поймаем ЛКМ.
        if (_listening == null || Time.frameCount == _listenStartFrame)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopListening(true);
            return;
        }

        foreach (KeyCode key in BindableKeys)
        {
            if (!Input.GetKeyDown(key))
                continue;

            BindingRow row = _listening;
            StopListening(false);
            _waitForRelease = key;
            SetUiBlocked(true);

            if (KeyBindings.Get(row.Action) == key)
                ShowKey(row, true);
            else
                KeyBindings.Set(row.Action, key);
            return;
        }
    }

    #region Listening

    private void StartListening(BindingRow row)
    {
        if (_waitForRelease != KeyCode.None)
            return;

        StopListening(true);
        _listening = row;
        _listenStartFrame = Time.frameCount;
        KeyBindings.IsListening = true;
        SetUiBlocked(true);
        PlayEffect(row, WaitingEffect(row.KeyText));

        // Иначе Space/Enter, нажатые для бинда, придут кнопке ещё и как Submit и снова запустят ожидание.
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    private void StopListening(bool restoreText)
    {
        if (_listening == null)
            return;

        BindingRow row = _listening;
        _listening = null;
        KeyBindings.IsListening = false;
        KeyBindings.ListeningEndFrame = Time.frameCount;

        if (_waitForRelease == KeyCode.None)
            SetUiBlocked(false);

        if (restoreText)
            ShowKey(row, true);
    }

    private void SetUiBlocked(bool blocked)
    {
        if (_inputBlocker != null)
            _inputBlocker.blocksRaycasts = !blocked;
    }

    private void OnBindingsChanged()
    {
        foreach (BindingRow row in _rows)
        {
            if (row != _listening && row.Shown != KeyBindings.Get(row.Action))
                ShowKey(row, true);
        }
    }

    private void ResetBindings()
    {
        StopListening(true);
        KeyBindings.ResetToDefaults();
    }

    #endregion

    #region Effects

    private void ShowKey(BindingRow row, bool animate)
    {
        row.Shown = KeyBindings.Get(row.Action);
        string target = KeyBindings.GetDisplayName(row.Shown);

        if (animate && isActiveAndEnabled)
        {
            PlayEffect(row, DecodeEffect(row.KeyText, target));
            row.KeyText.transform.DOKill(true);
            row.KeyText.transform.DOPunchScale(Vector3.one * 0.18f, 0.3f, 7)
                .SetUpdate(true)
                .SetLink(row.KeyText.gameObject);
            return;
        }

        StopEffect(row);
        row.KeyText.text = target;
        row.KeyText.alpha = 1f;
    }

    private void PlayEffect(BindingRow row, IEnumerator routine)
    {
        StopEffect(row);
        if (isActiveAndEnabled)
            row.Effect = StartCoroutine(routine);
    }

    private void StopEffect(BindingRow row)
    {
        if (row.Effect != null)
            StopCoroutine(row.Effect);
        row.Effect = null;
    }

    private IEnumerator WaitingEffect(TMP_Text text)
    {
        while (true)
        {
            text.text = Glitch("> " + _pressKeyText.ToUpperInvariant() + " <", 0.12f);
            text.alpha = 0.45f + 0.55f * Mathf.Abs(Mathf.Sin(Time.unscaledTime * 7f));
            yield return new WaitForSecondsRealtime(GlitchFrame);
        }
    }

    private IEnumerator DecodeEffect(TMP_Text text, string target)
    {
        text.alpha = 1f;
        float start = Time.unscaledTime;
        StringBuilder builder = new StringBuilder(target.Length);

        while (Time.unscaledTime - start < DecodeDuration)
        {
            int revealed = Mathf.FloorToInt((Time.unscaledTime - start) / DecodeDuration * target.Length);
            builder.Clear();
            for (int i = 0; i < target.Length; i++)
                builder.Append(i < revealed || target[i] == ' ' ? target[i] : RandomGlyph());

            text.text = builder.ToString();
            yield return new WaitForSecondsRealtime(GlitchFrame * 0.6f);
        }

        text.text = target;
    }

    private static string Glitch(string source, float chance)
    {
        StringBuilder builder = new StringBuilder(source.Length);
        foreach (char symbol in source)
            builder.Append(symbol != ' ' && Random.value < chance ? RandomGlyph() : symbol);
        return builder.ToString();
    }

    private static char RandomGlyph()
    {
        return GlitchGlyphs[Random.Range(0, GlitchGlyphs.Length)];
    }

    #endregion

    #region Building

    private GameObject BuildPanel(Transform generalPanel, Transform rowTemplate)
    {
        // Клонируем под выключенным родителем: Awake/Start у клонов (выбор языка, слайдеры и т.п.)
        // не должны сработать до того, как мы их вычистим.
        GameObject builder = new GameObject("KeyBindingsBuilder");
        builder.SetActive(false);

        try
        {
            GameObject panel = Instantiate(generalPanel.gameObject, builder.transform, false);
            panel.name = "ControlsSettings";
            RectTransform content = PrepareContent(panel);
            Transform template = Instantiate(rowTemplate.gameObject, builder.transform, false).transform;
            StripLocalization(template.gameObject);

            foreach (SectionInfo section in Sections)
            {
                AddHeaderRow(content, template, section);
                foreach (BindingInfo binding in section.Bindings)
                    AddBindingRow(content, template, binding);
            }

            AddResetRow(content, template);
            FitContentHeight(content);

            panel.SetActive(false);
            panel.transform.SetParent(generalPanel.parent, false);
            panel.transform.SetSiblingIndex(generalPanel.GetSiblingIndex() + 1);
            return panel;
        }
        finally
        {
            Destroy(builder);
        }
    }

    private RectTransform PrepareContent(GameObject panel)
    {
        ScrollRect scroll = FindShallowest<ScrollRect>(panel.transform);
        RectTransform content;

        if (scroll != null && scroll.content != null)
        {
            content = scroll.content;
            ClearChildren(content);
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, 0);
        }
        else
        {
            ClearChildren(panel.transform);
            RectTransform viewport = (RectTransform)panel.transform;
            panel.AddComponent<RectMask2D>();
            if (panel.GetComponent<Graphic>() == null)
                panel.AddComponent<Image>().color = Color.clear;

            content = new GameObject("Content", typeof(RectTransform)).GetComponent<RectTransform>();
            content.SetParent(viewport, false);
            content.anchorMin = new Vector2(0, 1);
            content.anchorMax = new Vector2(1, 1);
            content.pivot = new Vector2(0.5f, 1);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = Vector2.zero;

            scroll = panel.AddComponent<ScrollRect>();
            scroll.viewport = viewport;
            scroll.content = content;
            scroll.horizontal = false;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = 40;
        }

        if (content.GetComponent<VerticalLayoutGroup>() == null)
        {
            VerticalLayoutGroup layout = content.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
        }

        return content;
    }

    private void AddHeaderRow(RectTransform content, Transform template, SectionInfo section)
    {
        GameObject row = Instantiate(template.gameObject, content, false);
        row.name = "Header_" + section.Fallback;
        GetRowParts(row.transform, out TMP_Text label, out Button button, out _);
        DestroyImmediate(button.gameObject);

        PrepareText(label);
        label.alpha = 0.6f;
        label.characterSpacing += 6f;
        LocalizeTo(section.Key, section.Fallback, value => label.text = "// " + value.ToUpperInvariant());
    }

    private void AddBindingRow(RectTransform content, Transform template, BindingInfo binding)
    {
        GameObject row = Instantiate(template.gameObject, content, false);
        row.name = "Bind_" + binding.Action;
        GetRowParts(row.transform, out TMP_Text label, out Button button, out TMP_Text keyText);

        PrepareText(label);
        PrepareText(keyText);
        LocalizeTo(binding.Key, binding.Fallback, value => label.text = value);

        BindingRow bindingRow = new BindingRow { Action = binding.Action, KeyText = keyText };
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(() => StartListening(bindingRow));
        _rows.Add(bindingRow);
    }

    private void AddResetRow(RectTransform content, Transform template)
    {
        GameObject row = Instantiate(template.gameObject, content, false);
        row.name = "ResetBindings";
        GetRowParts(row.transform, out TMP_Text label, out Button button, out TMP_Text buttonText);

        PrepareText(label);
        PrepareText(buttonText);
        LocalizeTo("bind_reset", "Reset controls", value => label.text = value);
        LocalizeTo("bind_reset_button", "Reset", value => buttonText.text = value.ToUpperInvariant());

        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(ResetBindings);
    }

    private void BuildTab(List<RectTransform> tabs, GameObject panel)
    {
        RectTransform template = tabs[tabs.Count - 1];
        GameObject tab = Instantiate(template.gameObject, template.parent, false);
        tab.name = "ControlsButton";
        tab.transform.SetSiblingIndex(template.GetSiblingIndex() + 1);
        StripLocalization(tab);

        Button button = tab.GetComponent<Button>();
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(() =>
        {
            StopListening(true);
            _switcher.ChoosePanel(panel);
        });

        TMP_Text label = tab.GetComponentInChildren<TMP_Text>(true);
        if (label != null)
            LocalizeTo("controls", "Controls", value => label.text = value);

        LayoutTabs(tabs, (RectTransform)tab.transform);
    }

    // Справа места под ещё одну вкладку нет, поэтому ряд растёт влево: новая вкладка встаёт на место
    // последней, остальные сдвигаются на шаг. Заголовок и линия под ним центрируются по новому ряду,
    // разделители между вкладками раскладываются заново.
    private void LayoutTabs(List<RectTransform> oldTabs, RectTransform newTab)
    {
        RectTransform last = oldTabs[oldTabs.Count - 1];
        float tabWidth = last.rect.width * Mathf.Abs(last.localScale.x);
        float tabHeight = last.rect.height * Mathf.Abs(last.localScale.y);
        float step = oldTabs.Count > 1
            ? oldTabs[1].anchoredPosition.x - oldTabs[0].anchoredPosition.x
            : tabWidth * 1.05f;
        float tabY = last.anchoredPosition.y;
        float oldCenter = (oldTabs[0].anchoredPosition.x + last.anchoredPosition.x) * 0.5f;

        List<RectTransform> separators = new List<RectTransform>();
        List<RectTransform> header = new List<RectTransform>();
        foreach (Transform child in last.parent)
        {
            RectTransform rect = child as RectTransform;
            if (rect == null || rect == newTab || oldTabs.Contains(rect))
                continue;

            float width = rect.rect.width * Mathf.Abs(rect.localScale.x);
            bool isSeparator = child.GetComponent<Image>() != null
                               && Mathf.Abs(rect.anchoredPosition.y - tabY) < tabHeight
                               && width < tabWidth * 0.5f;
            if (isSeparator)
                separators.Add(rect);
            else
                header.Add(rect);
        }

        newTab.anchoredPosition = last.anchoredPosition;
        foreach (RectTransform tab in oldTabs)
            tab.anchoredPosition -= new Vector2(step, 0);

        List<RectTransform> allTabs = new List<RectTransform>(oldTabs) { newTab };
        float newCenter = (allTabs[0].anchoredPosition.x + newTab.anchoredPosition.x) * 0.5f;

        foreach (RectTransform rect in header)
        {
            rect.anchoredPosition += new Vector2(newCenter - oldCenter, 0);
            bool isUnderline = rect.GetComponent<Image>() != null
                               && rect.rect.width * Mathf.Abs(rect.localScale.x) >= tabWidth * 2f
                               && rect.rect.height <= 12f;
            if (isUnderline && Mathf.Approximately(rect.anchorMin.x, rect.anchorMax.x))
                rect.sizeDelta += new Vector2(step / Mathf.Abs(rect.localScale.x), 0);
        }

        if (separators.Count == 0)
            return;

        separators.Sort((a, b) => a.anchoredPosition.x.CompareTo(b.anchoredPosition.x));
        while (separators.Count < allTabs.Count - 1)
        {
            RectTransform clone = Instantiate(separators[0].gameObject, separators[0].parent, false)
                .GetComponent<RectTransform>();
            clone.name = separators[0].name + " (controls)";
            separators.Add(clone);
        }

        for (int i = 0; i < separators.Count; i++)
        {
            int left = Mathf.Min(i, allTabs.Count - 2);
            float x = (allTabs[left].anchoredPosition.x + allTabs[left + 1].anchoredPosition.x) * 0.5f;
            separators[i].anchoredPosition = new Vector2(x, separators[i].anchoredPosition.y);
        }
    }

    private static void FitContentHeight(RectTransform content)
    {
        VerticalLayoutGroup layout = content.GetComponent<VerticalLayoutGroup>();
        float height = layout.padding.top + layout.padding.bottom;
        for (int i = 0; i < content.childCount; i++)
        {
            height += ((RectTransform)content.GetChild(i)).rect.height;
            if (i > 0)
                height += layout.spacing;
        }

        content.sizeDelta = new Vector2(content.sizeDelta.x, height);
    }

    private static void PrepareText(TMP_Text text)
    {
        // Длинные переводы ужимаются, а не переносятся на вторую строку.
        text.enableWordWrapping = false;
        text.fontSizeMax = text.fontSize;
        text.fontSizeMin = text.fontSize * 0.5f;
        text.enableAutoSizing = true;
    }

    private void LocalizeTo(string key, string fallback, Action<string> apply)
    {
        apply(fallback);

        LocalizedString localized = new LocalizedString(TableName, key);
        LocalizedString.ChangeHandler handler = value =>
        {
            bool missing = string.IsNullOrEmpty(value) || value.StartsWith("No translation found");
            apply(missing ? fallback : value);
        };

        localized.StringChanged += handler;
        _unsubscribers.Add(() => localized.StringChanged -= handler);
    }

    #endregion

    #region Hierarchy helpers

    private static List<RectTransform> GetTabs(Transform switcher)
    {
        List<RectTransform> tabs = new List<RectTransform>();
        foreach (Transform child in switcher)
        {
            if (child is RectTransform rect && child.GetComponent<Button>() != null)
                tabs.Add(rect);
        }

        tabs.Sort((a, b) => a.anchoredPosition.x.CompareTo(b.anchoredPosition.x));
        return tabs;
    }

    // Строка-образец: подпись + кнопка с текстом (как "Blood Splat").
    private static Transform FindRowTemplate(Transform root)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(root);
        while (queue.Count > 0)
        {
            Transform current = queue.Dequeue();
            if (current != root && IsRow(current))
                return current;

            foreach (Transform child in current)
                queue.Enqueue(child);
        }

        return null;
    }

    private static bool IsRow(Transform transform)
    {
        Button button = null;
        TMP_Text label = null;
        foreach (Transform child in transform)
        {
            Button childButton = child.GetComponent<Button>();
            if (childButton != null && button == null && child.GetComponentInChildren<TMP_Text>(true) != null)
                button = childButton;
            else if (childButton == null && child.GetComponent<Selectable>() == null && label == null)
                label = child.GetComponent<TMP_Text>();
        }

        return button != null && label != null;
    }

    private static void GetRowParts(Transform row, out TMP_Text label, out Button button, out TMP_Text buttonText)
    {
        label = null;
        button = null;
        foreach (Transform child in row)
        {
            Button childButton = child.GetComponent<Button>();
            if (childButton != null && button == null)
                button = childButton;
            else if (childButton == null && label == null)
                label = child.GetComponent<TMP_Text>();
        }

        buttonText = button.GetComponentInChildren<TMP_Text>(true);
    }

    private static Transform FindDeep(Transform root, string name)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(root);
        while (queue.Count > 0)
        {
            Transform current = queue.Dequeue();
            if (current.name == name)
                return current;

            foreach (Transform child in current)
                queue.Enqueue(child);
        }

        return null;
    }

    private static T FindShallowest<T>(Transform root) where T : Component
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(root);
        while (queue.Count > 0)
        {
            Transform current = queue.Dequeue();
            T component = current.GetComponent<T>();
            if (component != null)
                return component;

            foreach (Transform child in current)
                queue.Enqueue(child);
        }

        return null;
    }

    private static void ClearChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
            DestroyImmediate(parent.GetChild(i).gameObject);
    }

    private static void StripLocalization(GameObject root)
    {
        foreach (LocalizeStringEvent localizer in root.GetComponentsInChildren<LocalizeStringEvent>(true))
            DestroyImmediate(localizer);
    }

    #endregion
}
