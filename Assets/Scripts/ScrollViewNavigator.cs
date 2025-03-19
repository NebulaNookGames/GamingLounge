using System;
using UnityEngine;
using UnityEngine.UI; 

public class ScrollViewNavigator : MonoBehaviour
{ 
 [SerializeField] private int maxButtons;
 private int currentButton;
 private Vector3 targetPos;
 [SerializeField] private Vector3 elementStep;
 [SerializeField] private RectTransform rect;

 [SerializeField] private float tweenTime;
 [SerializeField] private LeanTweenType tweenType;

 [SerializeField] private Button previousButton;
 [SerializeField] private Button nextButton;
 
 private void Awake()
 {
  currentButton = 1; 
  targetPos = rect.localPosition;
 }

 public void Next()
 { 
  if (currentButton < maxButtons)
  { 
   currentButton++;
   targetPos += elementStep;
   Move();
  }
 }
 
 public void Previous()
 {
  if (currentButton > 1)
  {
   currentButton--;
   targetPos -= elementStep;
   Move();
  }
 }

 void Move()
 {
  rect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
  if (currentButton >= maxButtons)
  {
   nextButton.interactable = false;
  }
  else
  {
   nextButton.interactable = true;
  }

  if (currentButton <= 1)
  {
   previousButton.interactable = false;
  }
  else
  {
   previousButton.interactable = true;
  }
 }
  
}