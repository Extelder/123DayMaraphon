using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameAction
{
    MoveForward,
    MoveBack,
    MoveLeft,
    MoveRight,
    Jump,
    Dash,
    DashDown,
    Shoot,
    Ability,
    Katana,
    Ultimate,
    Weapon1,
    Weapon2,
    Weapon3
}

public static class KeyBindings
{
    private const string PrefsPrefix = "KeyBinding_";

    private static readonly Dictionary<GameAction, KeyCode> Defaults = new Dictionary<GameAction, KeyCode>
    {
        { GameAction.MoveForward, KeyCode.W },
        { GameAction.MoveBack, KeyCode.S },
        { GameAction.MoveLeft, KeyCode.A },
        { GameAction.MoveRight, KeyCode.D },
        { GameAction.Jump, KeyCode.Space },
        { GameAction.Dash, KeyCode.LeftShift },
        { GameAction.DashDown, KeyCode.LeftControl },
        { GameAction.Shoot, KeyCode.Mouse0 },
        { GameAction.Ability, KeyCode.Mouse1 },
        { GameAction.Katana, KeyCode.F },
        { GameAction.Ultimate, KeyCode.R },
        { GameAction.Weapon1, KeyCode.Alpha1 },
        { GameAction.Weapon2, KeyCode.Alpha2 },
        { GameAction.Weapon3, KeyCode.Alpha3 },
    };

    private static Dictionary<GameAction, KeyCode> _current;

    public static readonly GameAction[] Actions = (GameAction[])Enum.GetValues(typeof(GameAction));

    public static event Action Changed;

    // Пока меню ждёт клавишу, Esc отменяет ожидание, а не закрывает настройки.
    public static bool IsListening { get; set; }
    public static int ListeningEndFrame { get; set; } = -1;

    public static bool EscapeConsumed => IsListening || ListeningEndFrame == Time.frameCount;

    public static KeyCode Get(GameAction action)
    {
        EnsureLoaded();
        return _current[action];
    }

    public static float GetAxis(GameAction positive, GameAction negative)
    {
        float value = 0;
        if (Input.GetKey(Get(positive)))
            value += 1;
        if (Input.GetKey(Get(negative)))
            value -= 1;
        return value;
    }

    // Возвращает действие, у которого забрали клавишу (оно получает старую клавишу взамен).
    public static GameAction? Set(GameAction action, KeyCode key)
    {
        EnsureLoaded();
        KeyCode old = _current[action];
        if (old == key)
            return null;

        GameAction? swapped = null;
        foreach (GameAction other in Actions)
        {
            if (other == action || _current[other] != key)
                continue;

            _current[other] = old;
            Save(other);
            swapped = other;
        }

        _current[action] = key;
        Save(action);
        PlayerPrefs.Save();
        Changed?.Invoke();
        return swapped;
    }

    public static void ResetToDefaults()
    {
        EnsureLoaded();
        foreach (GameAction action in Actions)
        {
            _current[action] = Defaults[action];
            PlayerPrefs.DeleteKey(PrefsPrefix + action);
        }

        PlayerPrefs.Save();
        Changed?.Invoke();
    }

    public static bool CanBind(KeyCode key)
    {
        return key != KeyCode.None && key != KeyCode.Escape && key < KeyCode.JoystickButton0;
    }

    public static string GetDisplayName(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Mouse0: return "LMB";
            case KeyCode.Mouse1: return "RMB";
            case KeyCode.Mouse2: return "MMB";
            case KeyCode.Mouse3: return "MOUSE 4";
            case KeyCode.Mouse4: return "MOUSE 5";
            case KeyCode.Mouse5: return "MOUSE 6";
            case KeyCode.Mouse6: return "MOUSE 7";
            case KeyCode.LeftShift: return "L-SHIFT";
            case KeyCode.RightShift: return "R-SHIFT";
            case KeyCode.LeftControl: return "L-CTRL";
            case KeyCode.RightControl: return "R-CTRL";
            case KeyCode.LeftAlt: return "L-ALT";
            case KeyCode.RightAlt: return "R-ALT";
            case KeyCode.LeftCommand: return "L-CMD";
            case KeyCode.RightCommand: return "R-CMD";
            case KeyCode.Return: return "ENTER";
            case KeyCode.KeypadEnter: return "NUM ENTER";
            case KeyCode.CapsLock: return "CAPS";
            case KeyCode.BackQuote: return "`";
            case KeyCode.Minus: return "-";
            case KeyCode.Equals: return "=";
            case KeyCode.LeftBracket: return "[";
            case KeyCode.RightBracket: return "]";
            case KeyCode.Semicolon: return ";";
            case KeyCode.Quote: return "'";
            case KeyCode.Backslash: return "\\";
            case KeyCode.Comma: return ",";
            case KeyCode.Period: return ".";
            case KeyCode.Slash: return "/";
            case KeyCode.UpArrow: return "UP";
            case KeyCode.DownArrow: return "DOWN";
            case KeyCode.LeftArrow: return "LEFT";
            case KeyCode.RightArrow: return "RIGHT";
        }

        if (key >= KeyCode.Alpha0 && key <= KeyCode.Alpha9)
            return ((int)(key - KeyCode.Alpha0)).ToString();
        if (key >= KeyCode.Keypad0 && key <= KeyCode.Keypad9)
            return "NUM " + (int)(key - KeyCode.Keypad0);

        return key.ToString().ToUpperInvariant();
    }

    private static void EnsureLoaded()
    {
        if (_current != null)
            return;

        _current = new Dictionary<GameAction, KeyCode>();
        foreach (GameAction action in Actions)
        {
            KeyCode key = (KeyCode)PlayerPrefs.GetInt(PrefsPrefix + action, (int)Defaults[action]);
            _current[action] = Enum.IsDefined(typeof(KeyCode), key) && CanBind(key) ? key : Defaults[action];
        }
    }

    private static void Save(GameAction action)
    {
        PlayerPrefs.SetInt(PrefsPrefix + action, (int)_current[action]);
    }
}
