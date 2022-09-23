using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RaySearch : MonoBehaviour
{
    public float stepSize = 0.1f;
    public float offsetMargin = 0.01f;
    public int checkCountMax = 100;

    List<Vector3[]> debugTangentCheck = new List<Vector3[]>();
    List<Vector3[]> debugNegativeCheck = new List<Vector3[]>();
    List<Vector3[]> debugBehindCheck = new List<Vector3[]>();
    [SerializeField] List<MeshPoint> meshPoints = new List<MeshPoint>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            DoPoints();
        }
    }
    void DoPoints()
    {
        if (Physics.Raycast(transform.position, Vector3.forward*2f, out RaycastHit hit))
        {
            if(hit.transform.CompareTag("ENEMY"))
            {
                Debug.Log("Enemy ilk Ray");
                FindNext(hit.point, hit.normal);
               
            }
            
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.forward * 2f);

        Handles.color = Color.white;
        DrawLinePairs(debugTangentCheck, Color.red);
        DrawLinePairs(debugNegativeCheck, Color.blue);
        DrawLinePairs(debugBehindCheck, Color.cyan);
       
    }

    void DrawLinePairs(List<Vector3[]> list, Color color)
    {
        Handles.color = color;
        foreach (Vector3[] pair in list)
        {
            Handles.DrawAAPolyLine(pair[0], pair[1]);
        }
    }
    [SerializeField] bool foundThing = false;
   
    void FindNext(Vector3 pt, Vector3 normal)
    {
        MeshPoint mp = new MeshPoint(); mp.position = pt; mp.normal = normal;
        meshPoints.Add(mp);
      
        //ilk bir ray atıyorum. O ray çarptıysa ve koşulumu sağlıyorsa cross alıp teğetini alıyorum.
        Vector3 tangent = Vector3.Cross(normal, Vector3.up); //teget bu yönü
        Vector3 offsetPt = pt + normal * offsetMargin;// bu offset veriyorum yapışmasın diye. daha geri bir nokta çarpığı yere göre
        Vector3 tangentCheckPoint = offsetPt + tangent * stepSize;
        Vector3 negativeCheckPoint = tangentCheckPoint - normal * (offsetMargin * 2);

        RaycastHit hit;

        
        if (Physics.Raycast(offsetPt, tangent, out hit, stepSize)) //offsetten teget yönünde stepsize kadar ışın atıyorum.
        {
            debugTangentCheck.Add(new[] { offsetPt, hit.point });
            foundThing = true;
        }
        else
        {
            debugTangentCheck.Add(new[] { offsetPt, tangentCheckPoint });// sağa attım kimse yok. devam ediyorum. listeye ekledim.
           


            if (Physics.Raycast(tangentCheckPoint, -normal, out hit, offsetMargin * 2))//sarı ray
            {
                if(hit.transform.CompareTag("ENEMY"))
                {
                    Debug.Log("Enemy bulduk kardeş");
                    debugNegativeCheck.Add(new[] { tangentCheckPoint, hit.point });
                    foundThing = true;
                }

            }
            else
            {
                debugNegativeCheck.Add(new[] { tangentCheckPoint, negativeCheckPoint });
                foundThing = false;


            }
        }


        if (foundThing && meshPoints.Count<checkCountMax)
        {
            FindNext(hit.point, hit.normal);
        }
        else if (!foundThing)
        {
            transform.position = Vector3.Lerp(transform.position, tangentCheckPoint, 1f);
        }

    }




}

public struct MeshPoint
{
    public Vector3 position;
    public Vector3 normal;
}

