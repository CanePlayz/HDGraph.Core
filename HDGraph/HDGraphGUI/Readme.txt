HDGraph (http://www.hdgraph.com)
=========================================

[French version at the bottom !]
[Version Française plus bas !]


Minimum requirements
======================================================

- Windows XP or Windows Vista or Windows 7 or Windows 8 or Any system with Mono (Linux, Mac OS, etc.)
- For Windows XP, the Microsoft .NET Framework 2.0 (HDGraph Setup wil propose you to download it, if it's not installed).
- Recommanded (but not required) : in order to use the evolved draw engine, you must have the version 3.5 SP1 of the .NET Framework.

	
Install on WINDOWS :
======================================================

- Make sure your computer satisfy the minimum requirements.
- Unzip the Zip file and start HDGraph.exe


Install on Linux, Mac OS : 
======================================================

- You must install Mono if it's not installed already. (Version 2.6.1 or higher is recommended. HDGraph has NOT been tested with an older version of Mono). Download Mono at http://mono-project.com .
- Unzip the HDGraph Zip file somewhere on your hard disk.
- Open a shell and execute, in the HDGraph folder, the following command (without quotes) :   "mono HDGraph.exe"
	

Uninstall on WINDOWS :
======================================================

First of all : 
- If you previously activated the HDGraph "windows explorer integration", it is better to deactivate it first (open HDGraph and go to the menu item "Tools > Explorer Integration >  Remove me from the explorer context menu").
- Make sure you closed all opened HDGraph windows.

Next :
- you just have to delete the HDGraph folder.


Known bugs and limitations:
======================================================

- HDGraph is unable to scan files and directories which full path is greater thant 255 characters.
- Using HDGraph on Mono is experimental :
-    Some feature are not available
-    There are many visual bug yet
-    The evolved draw engine (using WPF) is not and will never be available (because WPF is not implemented in Mono). Only the basic draw engine is available.

Contacts:
======================================================

Last informations about HDGraph and its contact informations can be found at :
http://www.hdgraph.com

	
	
	


	

/*****************************************************
*
*				VERSION FRANCAISE
*
******************************************************/


	
Configuration requise
======================================================

- Windows XP ou Windows Vista ou Windows 7 ou Windows 8 ou tout système compatible avec Mono (Linux, Mac OS, etc.)
- Pour Windows XP, le .NET Framework 2.0 (Le Setup HDGraph vous proposera de le télécharger automatiquement si vous ne l'avez pas).
- Recommandé (mais facultatif) : pour utilisé le dernier moteur de dessin WPF, vous devez avoir la version 3.5 SP1 du framework .NET.


Installation sous WINDOWS :
======================================================

- Assurez vous que votre ordinateur possède au moins la configuration requise pour installer le logiciel.
- Décompressez le fichier ZIP dans un dossier puis lancez le fichier HDGraph.exe.

Installation sous Linux, Mac OS :
======================================================

- Vous devez installer Mono s'il n'est pas déjà installé. La version 2.6.1 ou supérieure est recommandée. HDGraph n'a PAS été testé avec une version antérieur de Mono. Mono est disponible sur http://mono-project.com .
- Décompresser le fichier Zip de HDGraph sur le disque dur.
- Ouvrir un shell, se positionner dans le répertoire où a été extrait le Zip HDGraph, et exécuter la commande suivante (sans les guillemets) :   "mono HDGraph.exe"


Désinstallation sous WINDOWS:
======================================================

Avant toute chose :
- Fermez toutes les fenêtres HDGraph ouvertes.
- Si vous avez dans le passé activé l'intégration d'HDGraph à l'explorateur, il est préférable de désactiver cette option avant de désinstaller.(ouvrir HDGraph and go to the menu item "Tools > Explorer Integration >  Remove me from the explorer context menu").

Ensuite :
- il vous suffit de supprimer le dossier dans lequel vous avez décompressé le fichier.



Bug connus et limitations :
======================================================

- HDGraph est incapable de scaner les fichiers et répetoires dont le chemin excède 255 caractères.
- Sous Mono : version EXPERIMENTALE :
-	Le nombre de fonctionnalités est restreinte
-   Il y a encore de nombreux bugs visuels
-   Le moteur de dessin évolué (en WPF) n'est pas et sera jamais disponible (WPF n'est pas implémenté dans Mono). Seul le moteur de dessin basique fonctionne.


Contacts:
======================================================

Les dernières informations à propos de HDGraph sont disponibles sur le site internet: 
http://www.hdgraph.com
