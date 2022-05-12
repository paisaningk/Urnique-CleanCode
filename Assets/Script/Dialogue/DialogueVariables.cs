using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using Object = Ink.Runtime.Object;

public class DialogueVariables
{
   public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

   public DialogueVariables(TextAsset loadGlobals)
   {
      //Create the story
      Story globalVariablesStory = new Story(loadGlobals.text);

      variables = new Dictionary<string, Object>();
      foreach (var name in globalVariablesStory.variablesState)
      {
         Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
         variables.Add(name,value);
         //Debug.Log($"Initialized global dialogue variable {name} = {value}");
      }
   }
      
   public void StartListening(Story story)
   {
      //it's important that variablesToStory is before assigning the lister!
      VariablesToStory(story);
      story.variablesState.variableChangedEvent += VariableChanged;
   }

   public void StopListening(Story story)
   {
      story.variablesState.variableChangedEvent -= VariableChanged;
   }
   
   private void VariableChanged(string name, Ink.Runtime.Object value)
   {
      if (variables.ContainsKey(name))
      {
         variables.Remove(name);
         variables.Add(name,value);
      }
   }

   private void VariablesToStory(Story story)
   {
      foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
      {
         story.variablesState.SetGlobal(variable.Key,variable.Value);
      }
   }
}
