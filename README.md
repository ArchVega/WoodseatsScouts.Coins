# Woodseats.Coins

## Requirements
install-module sqlserver (for powershell db scripts)

## Database Schema

Troops has many Members

Members has many ScavengeResults

ScavengeResults has many ScavengedCoins

### Notes
#### Members
* Code column is calculated.
* There is no Section table contains SectionId (eg "B", "C") and SectionName (eg, "Beavers", "Cubs", respectively). Only the SectionId is stored and a corresponding name is generated in code. This cannot be changed at the moment as the Code field is autocalculated using the Char value of Section.
* HasImage is used only to store the initial state of members, ie they all do not have photos. The app changes this value to true once an image is saved. If an image is deleted from the disk, the value in the DB does not revert back to false.

### ScavengeResults
* This table links a batch of scavenged coins to a member. This is needed to facilitate group scores.
  
### ScavengedCoins
* Links all scanned coins to a member via the ScavengeResult table.