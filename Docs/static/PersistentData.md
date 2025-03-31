# PersistentData
Inherits from object
## Initialization
<span style="color:red;">This class is abstract and cannot be instantiated.</span>
## Static Methods
#### function <span style="color:yellow;">SetProperty</span>(property: <span style="color:blue;">string</span>, value: <span style="color:blue;">Object</span>) → <span style="color:blue;">null</span>
> Sets the property with given name to the object value. Valid value types are float, string, bool, and int.

#### function <span style="color:yellow;">GetProperty</span>(property: <span style="color:blue;">string</span>, defaultValue: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Gets the property with given name. If property does not exist, returns defaultValue.

#### function <span style="color:yellow;">LoadFromFile</span>(fileName: <span style="color:blue;">string</span>, encrypted: <span style="color:blue;">bool</span>) → <span style="color:blue;">null</span>
> Loads persistent data from given file name. If encrypted is true, will treat the file as having been saved as encrypted.

#### function <span style="color:yellow;">SaveToFile</span>(fileName: <span style="color:blue;">string</span>, encrypted: <span style="color:blue;">bool</span>) → <span style="color:blue;">null</span>
> Saves current persistent data to given file name. If encrypted is true, will also encrypt the file instead of using plaintext.

#### function <span style="color:yellow;">Clear</span>() → <span style="color:blue;">null</span>
> Clears current persistent data.

#### function <span style="color:yellow;">IsValidFileName</span>(fileName: <span style="color:blue;">string</span>) → <span style="color:blue;">bool</span>
> Determines whether or not the given fileName will be allowed for use when saving/loading a file.

#### function <span style="color:yellow;">FileExists</span>(fileName: <span style="color:blue;">string</span>) → <span style="color:blue;">bool</span>
> Determines whether the file given already exists. Throws an error if given an invalid file name.


---

