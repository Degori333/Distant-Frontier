using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class PlayerControllerX : MonoBehaviour {

	public Transform rightGunBone;
	public Transform leftGunBone;
	public Arsenal[] arsenal;

	private Animator animator;

	void Awake() {
		animator = GetComponent<Animator> ();
		if (arsenal.Length > 0)
			SetArsenal (arsenal[0].name);
		}

	public void SetArsenal(string name) {
		foreach (Arsenal hand in arsenal) {
			if (hand.name == name) {
				if (hand.rightGun != null) {
					GameObject newRightGun = GameObject.Find(hand.rightGun.name);
					newRightGun.transform.parent = rightGunBone;
					newRightGun.transform.localPosition = new Vector3(0, -0.03f, 0.02f);
					newRightGun.transform.localRotation = Quaternion.Euler(270, 0, 180);
					if (hand.rightGun.name == "Sniper Rifle")
                    {
						newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
					}
					}
				if (hand.leftGun != null) {
					GameObject newLeftGun = GameObject.Find(hand.leftGun.name);
					newLeftGun.transform.parent = leftGunBone;
					newLeftGun.transform.localPosition = new Vector3(0, -0.03f, 0.025f);
					newLeftGun.transform.localRotation = Quaternion.Euler(270, 0, 180);
				}
				animator.runtimeAnimatorController = hand.controller;
				return;
				}
		}
	}

	[System.Serializable]
	public struct Arsenal {
		public string name;
		public GameObject rightGun;
		public GameObject leftGun;
		public RuntimeAnimatorController controller;
	}
}
