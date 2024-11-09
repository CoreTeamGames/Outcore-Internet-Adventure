using System.Collections;
using UnityEngine;

public class DashTrailRenderer : MonoBehaviour
{
	[SerializeField]
	private TrailRenderer _trailRenderer;

	[SerializeField]
	private Rigidbody2D _rigidbody;

	private bool _forceRender;

	public TrailRenderer trailRenderer => _trailRenderer;

	public void CreateTrail()
	{
		StopAllCoroutines();
		StartCoroutine(_CreateTrail());
	}

	private IEnumerator _CreateTrail()
	{
		_forceRender = true;
		yield return new WaitForSeconds(0.1f);
		_forceRender = false;
	}

	private void FixedUpdate()
	{
		_trailRenderer.emitting = _forceRender || _rigidbody.velocity.magnitude > 10f;
	}

	public void ClearTrail()
	{
		_trailRenderer.Clear();
	}
}
