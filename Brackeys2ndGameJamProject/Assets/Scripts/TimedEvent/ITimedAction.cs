using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Timed Action
// Summary : The base class of any "you gotta click this" or "you gotta press this button" action
public interface ITimedAction
{
  public bool Started { get; }

  public bool Resolved { get; }

  public void StartAction();

  public IEnumerator AnimateThenDestroySelf();

  //Assumedly this either has more parameters or the inherited members use class variables to return something.
  public int EvaluatePoints();
}

