using UnityEngine;
using System.Collections;

public class EctManager : MonoBehaviour
{
    public virtual bool Init() { return true; }
    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}
