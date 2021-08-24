using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Timed Action
// Summary : The base class of any "you gotta click this" or "you gotta press this button" action
public interface ITimedAction
{
  public IEnumerator StartAction();

  public IEnumerator AnimateThenDestroySelf();


  // Might 
  public void Resolve(ResolveState resolveState);

  //Assumedly this either has more parameters or the inherited members use class variables to return something.
  public int EvaluatePoints();
}

public enum ResolveState
{
  Succeeded, 
  Failed,   // When the user has missed the action or mistimed it
  Cancelled // When the user loses or maybe when something else happens
}

