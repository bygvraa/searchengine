Version 1: 25/8-2023

A seachengine that consist of an indexer and a search program.

The indexer will crawl a folder (in depth) and create a reverse index
in a database. It will only index all text files with .txt as extension.

The search program is a console program that offers a query-based search
in the reverse index. It is in the ConsoleSearch project.

The class library CommonStuff contains types that are used by the indexer
and the ConsoleSearch. It contains:

- config.cs that contains path for indexer and path for the database
- a type for a document

The project Renamer is a console program used to rename all files in a
folder. Current version will rename all files with no extension to have
.txt as extension.



