using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace TMPro.Examples
{
    
    public class Benchmark03 : MonoBehaviour
    {

        [FormerlySerializedAs("SpawnType")] public int spawnType = 0;
        [FormerlySerializedAs("NumberOfNPC")] public int numberOfNpc = 12;

        [FormerlySerializedAs("TheFont")] public Font theFont;

        //private TextMeshProFloatingText floatingText_Script;

        void Awake()
        {

        }


        void Start()
        {
            for (int i = 0; i < numberOfNpc; i++)
            {
                if (spawnType == 0)
                {
                    // TextMesh Pro Implementation
                    //go.transform.localScale = new Vector3(2, 2, 2);
                    GameObject go = new GameObject(); //"NPC " + i);
                    //go.transform.position = new Vector3(Random.Range(-95f, 95f), 0.5f, Random.Range(-95f, 95f));

                    go.transform.position = new Vector3(0, 0, 0);
                    //go.renderer.castShadows = false;
                    //go.renderer.receiveShadows = false;
                    //go.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

                    TextMeshPro textMeshPro = go.AddComponent<TextMeshPro>();
                    //textMeshPro.FontAsset = Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TextMeshProFont)) as TextMeshProFont;
                    textMeshPro.alignment = TextAlignmentOptions.Center;
                    textMeshPro.fontSize = 96;

                    textMeshPro.text = "@";
                    textMeshPro.color = new Color32(255, 255, 0, 255);
                    //textMeshPro.Text = "!";


                    // Spawn Floating Text
                    //floatingText_Script = go.AddComponent<TextMeshProFloatingText>();
                    //floatingText_Script.SpawnType = 0;
                }
                else
                {
                    // TextMesh Implementation
                    GameObject go = new GameObject(); //"NPC " + i);
                    //go.transform.position = new Vector3(Random.Range(-95f, 95f), 0.5f, Random.Range(-95f, 95f));

                    go.transform.position = new Vector3(0, 0, 0);

                    TextMesh textMesh = go.AddComponent<TextMesh>();
                    textMesh.GetComponent<Renderer>().sharedMaterial = theFont.material;
                    textMesh.font = theFont;
                    textMesh.anchor = TextAnchor.MiddleCenter;
                    textMesh.fontSize = 96;

                    textMesh.color = new Color32(255, 255, 0, 255);
                    textMesh.text = "@";

                    // Spawn Floating Text
                    //floatingText_Script = go.AddComponent<TextMeshProFloatingText>();
                    //floatingText_Script.SpawnType = 1;
                }
            }
        }

    }
}
