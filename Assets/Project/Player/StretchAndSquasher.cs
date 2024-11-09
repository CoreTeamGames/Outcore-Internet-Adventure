using DG.Tweening;
using UnityEngine;

public class StretchAndSquasher : MonoBehaviour
{
	

	public void StretchAndSquash(Vector2 velocity, float stretchAmount = 0.5f, float squashAmount = 0.5f)
	{
		SquashAnimation(velocity, squashAmount);
		StretchAnimation(velocity, stretchAmount);
	}

	public void SquashAnimation(Vector2 velocity, float stretchAmount = 0.5f)
	{
		float t = Mathf.Clamp01(Mathf.Abs(velocity.y) / 15f);
		float b = 1f - stretchAmount;
		float b2 = 1f + stretchAmount;
		float targetScaleX = Mathf.Lerp(1f, b2, t);
		float targetScaleY = Mathf.Lerp(1f, b, t);
		gameObject.transform.DOScale(new Vector3(targetScaleX, targetScaleY, 1f), 0.1f).OnComplete(delegate
		{
			gameObject.transform.DOScale(new Vector3(targetScaleY, targetScaleX, 1f), 0.1f).OnComplete(delegate
			{
				gameObject.transform.DOScale(Vector3.one, 0.1f);
			});
		});
	}

	public void StretchAnimation(Vector2 velocity, float stretchAmount = 0.5f)
	{
		float t = Mathf.Clamp01(Mathf.Abs(velocity.x) / 15f);
		float b = 1f - stretchAmount;
		float b2 = 1f + stretchAmount;
		float y = Mathf.Lerp(1f, b, t);
		float x = Mathf.Lerp(1f, b2, t);
		gameObject.transform.DOScale(new Vector2(x, y), 0.1f).OnComplete(delegate
		{
			gameObject.transform.DOScale(Vector2.one, 0.1f);
		});
	}
}
