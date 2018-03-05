using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ResourcesTest
{
	private Object mPrefab;
	[Test]
	public void ResourcesTestSimplePasses() {
		// Use the Assert class to test conditions.
		
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator ResourcesTestWithEnumeratorPasses()
	{
		// Use the Assert class to test conditions.
		// yield to skip a frame

		yield return new WaitForSeconds(1.0f);

		mPrefab = Resources.Load("GameObject");

		var gameObj = Object.Instantiate(mPrefab as GameObject);

		yield return new WaitForSeconds(1.0f);

		// prefab cannot be unloaded
		Resources.UnloadAsset(mPrefab);
		
		yield return new WaitForSeconds(1.0f);

		Object.Destroy(gameObj);
	}
}
