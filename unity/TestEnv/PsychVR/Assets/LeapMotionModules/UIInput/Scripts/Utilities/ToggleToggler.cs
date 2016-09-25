using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Leap.Unity.InputModule {
  public class ToggleToggler : MonoBehaviour {
    public Text text;
    public UnityEngine.UI.Image image;
    public Color OnColor;
    public Color OffColor;

    public void SetToggle(Toggle toggle) {
      if (toggle.isOn) {
                GameObject.FindGameObjectsWithTag("toggle_obj")[0].GetComponent<Animator>().SetBool(text.text, true);
        text.color = Color.white;
        image.color = OnColor;
      } else {
                GameObject.FindGameObjectsWithTag("toggle_obj")[0].GetComponent<Animator>().SetBool(text.text, false);
                text.color = new Color(0.3f, 0.3f, 0.3f);
        image.color = OffColor;
      }
    }
  }
}