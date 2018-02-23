# TextTranslator
Simple GUI that translates text via Cognitive Services Translation Service

# Features
1)	Support translation of multiple languages that can be configured in the config file. By default de -> en and en -> de is configured.
2)	Translation of the a specified text.
3)	Translation can be triggered by "Tranalte", pressing "Alt+T" or by pressing Return. If translation should not happen on Return this can be disabled when "Translate on Returnis disabled. 
5)	UI is resizeable and the translation textboxes adjust on with form size. 
6)	UI shows if the current text has been translated or not. It shows "Changed" or "Translated" in the lower right corner.

# Configuration
  <appSettings>
    <add key="Lang1From" value="de"></add>
    <add key="Lang1To" value="en"></add>
    <add key="Lang1SubcriptionKey" value="subscriptionkey"/>
    <add key="Lang1Category" value="categorykey"></add>
    
    <add key="Lang2From" value="en"></add>
    <add key="Lang2To" value="de"></add>
    <add key="Lang2SubcriptionKey" value="subscriptionkey"/>
    <add key="Lang2Category" value="categorykey"></add>

    <add key="CognitiveAuthUri" value="https://api.cognitive.microsoft.com/sts/v1.0/issueToken"/>
    <add key="TranslatorUri" value="https://api.microsofttranslator.com/v2/Http.svc"/>
  </appSettings>

There are 4 parameters for each translation
-	LangXFrom = Sourcelanguage in ISO format
-	LangXTo = Destinationlanguage in ISO format
-	LangXSubscriptionKey = The cognitive services subscription key
-	LangXCategory = The category key from the translation system.

