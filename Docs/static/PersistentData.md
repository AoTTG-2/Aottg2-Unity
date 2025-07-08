# PersistentData
Inherits from Object

<mark style="color:red;">This class is static and cannot be instantiated.</mark>

> Store and retrieve persistent data. Persistent data can be saved and loaded from file. Supports float, int, string, and bool types.             Note that any game mode may use the same file names, so it is recommended that you choose unique file names when saving and loading.             Saved files are located in Documents/Aottg2/PersistentData.
## Static Methods
###### function <mark style="color:yellow;">SetProperty</mark>(property: <mark style="color:blue;">string</mark>, value: <mark style="color:blue;">Object</mark>)
> Sets the property with given name to the object value. Valid value types are float, string, bool, and int.

###### function <mark style="color:yellow;">GetProperty</mark>(property: <mark style="color:blue;">string</mark>, defaultValue: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">Object</mark>
> Gets the property with given name. If property does not exist, returns defaultValue.

###### function <mark style="color:yellow;">LoadFromFile</mark>(fileName: <mark style="color:blue;">string</mark>, encrypted: <mark style="color:blue;">bool</mark>)
> Loads persistent data from given file name. If encrypted is true, will treat the file as having been saved as encrypted.

###### function <mark style="color:yellow;">SaveToFile</mark>(fileName: <mark style="color:blue;">string</mark>, encrypted: <mark style="color:blue;">bool</mark>)
> Saves current persistent data to given file name. If encrypted is true, will also encrypt the file instead of using plaintext.

###### function <mark style="color:yellow;">Clear</mark>()
> Clears current persistent data.

###### function <mark style="color:yellow;">IsValidFileName</mark>(fileName: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">bool</mark>
> Determines whether or not the given fileName will be allowed for use when saving/loading a file.

###### function <mark style="color:yellow;">FileExists</mark>(fileName: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">bool</mark>
> Determines whether the file given already exists. Throws an error if given an invalid file name.


---

