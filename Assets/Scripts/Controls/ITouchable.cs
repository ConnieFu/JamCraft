using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITouchable
{
	void OnTouchBegin();
	void OnTouchHold(Vector2 position);
    void OnTouchEnd();
}
