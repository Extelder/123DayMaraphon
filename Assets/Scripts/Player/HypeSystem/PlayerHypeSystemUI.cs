using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerHypeSystemUI : MonoBehaviour
{
    [SerializeField] private PlayerHypeSystem _hypeSystem;
    [SerializeField] private float _updateRate;

    private List<GameObject> lineList = new List<GameObject>();

    private DD_DataDiagram m_DataDiagram;

    private float m_Input = 0f;
    private float h = 0;

    private void Start()
    {
        GameObject dd = GameObject.Find("DataDiagram");
        if (null == dd)
        {
            Debug.LogWarning("can not find a gameobject of DataDiagram");
            return;
        }

        m_DataDiagram = dd.GetComponent<DD_DataDiagram>();

        m_DataDiagram.PreDestroyLineEvent += (s, e) => { lineList.Remove(e.line); };

        AddALine();

        StartCoroutine(nameof(Updating));
    }

    public void OnAddLine()
    {
        AddALine();
    }

    private void AddALine()
    {
        if (null == m_DataDiagram)
            return;

        Color color = Color.HSVToRGB((h += 0.1f) > 1 ? (h - 1) : h, 0.8f, 0.8f);
        GameObject line = m_DataDiagram.AddLine(color.ToString(), color);
        if (null != line)
            lineList.Add(line);
    }

    private IEnumerator Updating()
    {
        while (true)
        {
            yield return new WaitForSeconds(_updateRate);
            ContinueInput(1);
        }
    }

    private void ContinueInput(float f)
    {
        if (null == m_DataDiagram)
            return;

        float d = 0f;
        foreach (GameObject l in lineList)
        {
            m_DataDiagram.InputPoint(l, new Vector2(0.5f,
                _hypeSystem.Current));
        }
    }
}