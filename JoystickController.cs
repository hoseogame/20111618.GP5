using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour
{
    [SerializeField]
    private Image m_Stick;
    private Vector3 orignPos;

	void Start ()
    {
        //orignPos = this.gameObject.GetComponent<Image>().rectTransform.position;

        orignPos = m_Stick.transform.position;
    }
	
    public void OnDrag()
    {
        //Touch m_touch = Input.GetTouch(0);
        if (m_Stick != null)
        {
            // m_Stick.rectTransform.position = m_touch.position;
        }

        //Vector3 dir = (orignPos - new Vector3(m_touch.position.x, orignPos.y, orignPos.z)).normalized;

        //float touchAreaRadius = Vector3.Distance(orignPos, new Vector3(m_touch.position.x, orignPos.y, orignPos.z));
    }

    public void OnEndDrag()
    {
        if(m_Stick != null)
        {
            m_Stick.rectTransform.position = orignPos;
        }
    }
}
