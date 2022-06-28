using System;
using UnityEngine;
using System.Collections;
using LiquidVolumeFX;
using Random = UnityEngine.Random;

namespace LiquidVolumeFX {
				
				public class LiquidLevelPouringSync : MonoBehaviour {

								public float fillSpeed = 0.01f;
								public float sinkFactor = 0.1f;
								LiquidVolume lv;
								Rigidbody rb;

								private bool isCollision;

								void Start ()
								{
												isCollision = false;
												rb = GetComponent<Rigidbody> ();
												lv = transform.parent.GetComponent<LiquidVolume> ();
												UpdateColliderPos ();
								}


								void OnParticleCollision (GameObject other) {
												if (lv.level < 1f) {
																lv.level += fillSpeed;
												}
												
												UpdateColliderPos ();
												UiManager.instance.fillBar.fillAmount  += lv.level * UiManager.instance.liquidCount;
												lv._speed = 2f;
												
												Taptic.Light();
												Invoke("SpeedChanger", 0.8f);


								}
					

								void UpdateColliderPos () {
												Vector3 pos = new Vector3 (transform.position.x, lv.liquidSurfaceYPosition - transform.localScale.y * 0.5f - sinkFactor, transform.position.z);
												rb.position = pos;
												if (lv.level >= 1f) {
																transform.localRotation = Quaternion.Euler (Random.value * 30 - 15, Random.value * 30 - 15, Random.value * 30 - 15);
												} else {
																transform.localRotation = Quaternion.Euler (0, 0, 0);
												}
												
												if (lv.level >= 1f) {
													transform.localRotation = Quaternion.Euler (Random.value * 30 - 15, Random.value * 30 - 15, Random.value * 30 - 15);
												} else {
													transform.localRotation = Quaternion.Euler (0, 0, 0);
												}
								}

								public void SpeedChanger()
								{
									lv._speed = 0f;
								}

			
				}
}
