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
 public RectTransform viewport;
 [SerializeField] private float tweenTime;
 [SerializeField] private LeanTweenType tweenType;

 [SerializeField] private Button previousButton;
 [SerializeField] private Button nextButton;
 
 ScrollRect scrollRect;
 [SerializeField] Button[] buttons; 
 private void Awake()
 {
  scrollRect = GetComponent<ScrollRect>();
  currentButton = 1; 
  targetPos = rect.localPosition;
 }

 private void Update()
 {
  UpdateButtonNavigation();
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

 void UpdateButtonNavigation()
 {
  foreach (Button btn in buttons)
  {
   if (IsButtonVisible(btn))
   {
    EnableButtonNavigation(btn);
   }
   else
   {
    DisableButtonNavigation(btn);
   }
  }
 }

 bool IsButtonVisible(Button button)
 {
  RectTransform btnRect = button.GetComponent<RectTransform>();

  // Convert button's position to viewport's local position
  Vector3 worldPos = btnRect.position;
  Vector3 localPos = viewport.InverseTransformPoint(worldPos);

  // Check if within viewport bounds
  Rect viewportRect = viewport.rect;
  return viewportRect.Contains(localPos);
 }

 void EnableButtonNavigation(Button btn)
 {
  Navigation navigation = btn.navigation;
  navigation.mode = Navigation.Mode.Automatic; // Enable normal navigation
  btn.navigation = navigation;
 }

 void DisableButtonNavigation(Button btn)
 {
  Navigation navigation = btn.navigation;
  navigation.mode = Navigation.Mode.None; // Disable navigation
  btn.navigation = navigation;
 }
  
}