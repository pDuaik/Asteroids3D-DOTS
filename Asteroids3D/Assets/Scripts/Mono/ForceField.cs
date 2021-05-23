using UnityEngine;
using UnityEngine.UI;

public class ForceField : MonoBehaviour
{
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.enabled = GameDataManager.singleton.shield ? true : false;
    }
}
