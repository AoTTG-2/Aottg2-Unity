using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadEMUI : MonoBehaviour
{
    [SerializeField] private GameObject EMUIPrefab;
    void Start() { GameObject go = Instantiate(EMUIPrefab); }
}
