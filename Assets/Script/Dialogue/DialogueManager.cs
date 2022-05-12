using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using Script.Spawn;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
   [Header("Dialogue UI")]
   [SerializeField] private GameObject Dialogue;
   [SerializeField] private TextMeshProUGUI DialogueText;
   [SerializeField] private TextMeshProUGUI NameText;
   [SerializeField] private Image ImageProfile;
   [SerializeField] private GameObject Heart;

   [Header("DialogueChoices")] 
   [SerializeField] private GameObject[] ChoiceGameObjects;
   private TextMeshProUGUI[] choiceTexts;

   [Header("Load Globals Text Asset")] 
   [SerializeField] private TextAsset LoadGlobals;
   // [SerializeField] private TextAsset GlobalsTextAsset;
   
   // [Header("Item")] 
   // [SerializeField] private TextMeshProUGUI MonsterText;
   // [SerializeField] private TextMeshProUGUI BookText;
   public TextMeshProUGUI Book;
   public TextMeshProUGUI Monter;
   private static DialogueManager instance;
   private Story currentStory;
   public bool DialoguePlaying;
   private DialogueVariables  DialogueVariables;

   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
      }
      else
      {
         Debug.LogWarning("Found more than one Dialogue Manager in scene");
         Destroy(gameObject);
      }
      

      //set dialogue set active false
      DialoguePlaying = false;
      Dialogue.SetActive(DialoguePlaying);
      DialogueVariables = new DialogueVariables(LoadGlobals);

      choiceTexts = new TextMeshProUGUI[ChoiceGameObjects.Length];
      for (var i = 0; i < ChoiceGameObjects.Length; i++)
      { 
         choiceTexts[i] = ChoiceGameObjects[i].GetComponentInChildren<TextMeshProUGUI>();
      }
   }

   private void Start()
   {
      Heart.SetActive(false);
      ((Ink.Runtime.IntValue) GetVariableState("Item")).value = SpawnPlayer.instance.Item;
      ((Ink.Runtime.IntValue) GetVariableState("Monster")).value = SpawnPlayer.instance.Monster;
   }

   public static DialogueManager GetInstance()
   {
      return instance;
   }

   private void Update()
   {
      Book.text = $"{((Ink.Runtime.IntValue) GetVariableState("Item")).value}";
      Monter.text = $"{((Ink.Runtime.IntValue) GetVariableState("Monster")).value}";
      if (!DialoguePlaying)
      {
         return;
      }

      if (currentStory.currentChoices.Count == 0 && Input.GetKeyDown(KeyCode.E))
      {
         ContinueStory();
      }

      if (Input.GetKeyDown(KeyCode.L))
      {
         ((Ink.Runtime.IntValue) GetVariableState("Item")).value += 20;
         ((Ink.Runtime.IntValue) GetVariableState("Monster")).value += 20;
      }
   }

   private void ContinueStory()
   {
      if (currentStory.canContinue) 
      {
         // set text for the current dialogue line
         DialogueText.text = currentStory.Continue();
         
         // display choices, if any, for this dialogue line
         DisplayChoices();
      }
      else 
      {
         StartCoroutine(ExitDialogueMode());
      }
   }

   public void EnterDialogueMode(TextAsset ink,string name,Sprite imageProfile,NpcType npcType)
   {
      NameText.text = $"{name}";
      ImageProfile.sprite = imageProfile;
      StartCoroutine(EnterDialogueDelay(ink));
      if (npcType == NpcType.CanFlirt)
      {
         Heart.SetActive(true);
      }
   }

   public IEnumerator EnterDialogueDelay(TextAsset ink)
   {
      yield return new WaitForSeconds(0.1f);
      currentStory = new Story(ink.text);
      DialoguePlaying = true;
      Dialogue.SetActive(DialoguePlaying);
      DialogueVariables.StartListening(currentStory);
      ContinueStory();
   }

   public IEnumerator ExitDialogueMode()
   {
      Dialogue.SetActive(false);
      yield return new WaitForSeconds(0.1f);
      DialogueVariables.StopListening(currentStory);
      DialoguePlaying = false;
      DialogueText.text = null;
      Heart.SetActive(false);
   }

   private void DisplayChoices()
   {
      List<Choice> currentChoices = currentStory.currentChoices;

      // defensive check to make sure our UI can support the number of choices coming in
      if (currentChoices.Count > ChoiceGameObjects.Length)
      {
         Debug.LogError("More choices were given than the UI can support. Number of choices given: " 
                        + currentChoices.Count);
      }

      int index = 0;
      // enable and initialize the choices up to the amount of choices for this line of dialogue
      foreach(Choice choice in currentChoices) 
      {
         ChoiceGameObjects[index].gameObject.SetActive(true);
         choiceTexts[index].text = choice.text;
         index++;
      }
      // go through the remaining choices the UI supports and make sure they're hidden
      for (int i = index; i < ChoiceGameObjects.Length; i++) 
      {
         ChoiceGameObjects[i].gameObject.SetActive(false);
      }

   }

   // private IEnumerator SelectFirstChoice()
   // {
   //    //Event System requires we clear it first, than wait
   //    //for at least one frame before we set the current selected object.
   //    EventSystem.current.SetSelectedGameObject(null);
   //    yield return new WaitForEndOfFrame();
   //    EventSystem.current.SetSelectedGameObject(ChoiceGameObjects[0].gameObject);
   // }

   public void MakeChoice(int choiceIndex)
   {
      currentStory.ChooseChoiceIndex(choiceIndex);
      ContinueStory();
   }

   public Ink.Runtime.Object GetVariableState(string variableName)
   {
      Ink.Runtime.Object variableValue = null;
      DialogueVariables.variables.TryGetValue(variableName, out variableValue);
      if (variableValue == null)
      {
         
         Debug.LogWarning("ink variable was found to be null" + variableName);
      } 
      return variableValue;
   }
}
