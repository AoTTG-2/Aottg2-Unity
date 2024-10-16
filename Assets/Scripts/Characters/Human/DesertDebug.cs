using UnityEngine;

public class DesertDebug : MonoBehaviour
{
    public Vector3 PlayerPos; // Posizione del giocatore (impostata dall'esterno)
    public Vector3 HookPos;   // Posizione del gancio (impostata dall'esterno)
    public float xLimit = 1f; // Limite per l'asse X

    // Funzione originale
    public void Init(Vector3 HookP) 
    {
        HookPos = HookP;
       //Debug.Log(HookPos);
    }
    // Metodo per disegnare l'area nella scena
    void OnDrawGizmos()
    {
        // Calcola i punti dell'area di rilevamento
        Vector3 center = HookPos; // Punto centrale tra il giocatore e il gancio
        Vector3 size = new Vector3(xLimit * 2, 1, xLimit * 2); // Dimensioni dell'area (larghezza e profondità)

        // Imposta il colore dei Gizmos
        Gizmos.color = Color.green; // Colore dell'area

        // Disegna un cubo per rappresentare l'area
        Gizmos.DrawCube(center, size);

    }
}
