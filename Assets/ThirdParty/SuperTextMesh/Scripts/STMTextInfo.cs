//Copyright (c) 2016-2023 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; //converting arrays to dictionaries
using System.IO; //for getting folders
///NOTE: only re-create STMTextInfo if RAW TEXT changes
#if UNITY_EDITOR
using UnityEditor; //for loading default stuff and menu thing
#endif
[System.Serializable]
public class STMTextContainer{
    public string text;
    public STMTextContainer(string text){
        this.text = text;
    }
}

//--------------------------------------------------
// Performance updates performed by RedVonix
// http://www.RedVonix.com/
//
// All comments from Red are marked with "RV:"
//--------------------------------------------------

[System.Serializable]
public class STMTextInfo
{ //info for an individual letter. used internally by super text mesh. References back to textdata. (info[i], per-letter)
	//MAKE SURE TO ADD VARIABLE TO CONSTRUCTOR WHEN MAKING A NEW ONE
	//public Font font; //what font this character info is... wait you have FontData, otherwise use mesh default
	public CharacterInfo ch; //contains uv data, point size (quality), style, etc
	//public char c; //nah, just sync it w/ hyphenedText[]. it works better cause hyphenedtext will always be shorter
	public Vector3 pos; //where the bottom-left corner is
	public Vector3 offset; //additional pos... for superscript text. implementing it this way cause just using pos initially caused weirdness

	public float lineSpacing; //just for you, tassy

	public bool isEndOfParagraph = false; //is this a typed-up linebreak? used for RTL text flipping. will never be copied.
	public float readTime = -1f; //at what time it will start to get read.
	public float unreadTime = -1f;
	public int line = 0; //what line this text is on
	public int rawIndex; //where this character is on the unfiltered text (_text)
	public float indent = 0f; //distance from left-oriented margin that a new row will start from
	public STMDrawAnimData drawAnimData; //which draw animation will be used
	public Vector2 size; //localspace size
	//public Vector3 offset = Vector3.zero; //offset of this character, independent of effects. meant for superscript text
	public SuperTextMesh.Alignment alignment; //how this text will be aligned
	public SuperTextMesh.DrawOrder drawOrder;
	//public Color32 color = Color.white; //the actual color it will render at, not the color data!
	//public int delayCount; //the delay count...

	public List<string> ev = new List<string>(); //event strings.
	public List<string> ev2 = new List<string>(); //repeating event strings.
	public STMColorData colorData; //reference or store...
	public STMGradientData gradientData; //reference stuff??
	public STMTextureData textureData;

	//vvvADDITIONAL delay!
	public STMDelayData delayData; //units to delay for, before this text is shown. multiple of read speed. ADDITIONAL DELAY
	
	public STMWaveData waveData;
	public STMJitterData jitterData;

//	public STMImageData imageData; //if not null, put this referenced image inline
	
	//public STMVoiceData voiceData; //if not null, this will override the text mesh's settings????? ??????
	public float readDelay = 0f; //delay between lettes, for setting up timing
	public STMAudioClipData audioClipData;
	public bool stopPreviousSound;
	public SuperTextMesh.PitchMode pitchMode;
	public float overridePitch;
	public float minPitch;
	public float maxPitch;
	public float speedReadPitch;
	public STMFontData fontData;
	public STMQuadData quadData;
	/*
	This value is assigned manually by STM when setting quadData
	so a null check doesn't have to be performed every time
	the value is set
	 */
	public bool isQuad = false; //cached value for fast checking!
	public int quadIndex = -1;
	public STMMaterialData materialData = null;
	public STMSoundClipData soundClipData;
    public int IdxIdentifier; // RV: Lookup Identifier.
    public bool InfoInUse; // RV: Shows if this identifier is being used or not.
    //public int submeshNumber = -1;

    private int chGlyphIndex = -1;
    public int chMinX;
    public int chMaxX;
    public int chMinY;
    public int chMaxY;
	private int chAdvance;
	private Vector2 chUvBottomLeft = Vector2.zero;
	private Vector2 chUvBottomRight = Vector2.zero;
	private Vector2 chUvTopLeft = Vector2.zero;
	private Vector2 chUvTopRight = Vector2.zero;
	//private int chBearing;
	public int chSize = 1; //quality

	public bool submeshChange = false;
	public bool invoked = false;
	//public FontStyle chStyle;

	//values grabbed from UICharInfo. Jeez
	//public float charWidth;
	//public Vector2 cursorPos = Vector2.zero;
	//^^^ having this value default like this fixes NaN values on solo linebreaks. ugh.

	public void UpdateCachedValuesIfChanged(bool force)
    {
        if(force || chGlyphIndex != ch.index)
        {
            chGlyphIndex = ch.index;
            chMinX = ch.minX;
            chMaxX = ch.maxX;
            chMinY = ch.minY;
            chMaxY = ch.maxY;
			chAdvance = ch.advance;
			chSize = ch.size;
			chUvBottomLeft = ch.uvBottomLeft;
			chUvBottomRight = ch.uvBottomRight;
			chUvTopLeft = ch.uvTopLeft;
			chUvTopRight = ch.uvTopRight;
		//	chStyle = ch.style;
			//chBearing = ch.bearing;
        }
		chSize = ch.size == 0 ? 1 : ch.size;
		//isQuad = _quadData != null;
    }

	public char character{
		get
		{
			/*
			//not needed right now
			if(chGlyphIndex > char.MaxValue || chGlyphIndex < char.MinValue)
			{
				return ''; //return null
			*/
			return System.Convert.ToChar(chGlyphIndex);
		}
	}
	public float uvHeight
    {
        get
        {
            if(chUvBottomLeft.x != chUvTopLeft.x)
            {
                //this thing is rotated!!!
                return chUvTopLeft.y - chUvTopRight.y;
            }
            return chUvBottomLeft.y - chUvTopLeft.y;
        }
    }
    public float uvWidth
    {
        get
        {
            if(chUvBottomRight.y != chUvBottomLeft.y)
            {
                //this thing is rotated!!!
                return chUvTopLeft.x - chUvBottomLeft.x;
            }
            return chUvBottomRight.x - chUvBottomLeft.x;
        }
    }
	private Vector2 uvMidReturn = Vector2.zero;
	public Vector2 uvMid
	{
		get
		{
			if (chUvTopLeft.x!=chUvBottomLeft.x){
				uvMidReturn.x = (chUvTopLeft.x + chUvBottomLeft.x) * 0.5f;
				uvMidReturn.y = (chUvTopLeft.y + chUvTopRight.y) * 0.5f;
			}
			else
			{
				uvMidReturn.x = (chUvTopLeft.x + chUvTopRight.x) * 0.5f;
				uvMidReturn.y = (chUvTopLeft.y + chUvBottomLeft.y) * 0.5f;
			}
			return uvMidReturn;
		}
	}

	//this code would work if the shader didn't care about rotation
	public Vector2 ratio
	{
		get
		{
			Vector2 ratioSize = Vector2.zero;

			if(isQuad)
			{ //use quad's ratio
				ratioSize.x = quadData.size.x;
				ratioSize.y = quadData.size.y;
			}
			else
			{ //use letter
				ratioSize.x = uvWidth;
				ratioSize.y = uvHeight;
			}
			return ratioSize;

		}
	}
	
	/*
	public Vector2 ratio{
		get{
			Vector2 ratioSize = Vector2.zero;

			if(isQuad)
			{ //use quad's ratio
				ratioSize.x = quadData.size.x;
				ratioSize.y = quadData.size.y;
			}
			else
			{ //use letter
				ratioSize.x = (float)chMaxX - (float)chMinX;
				ratioSize.y = (float)chMaxY - (float)chMinY;
			}

			if(ratioSize.x > ratioSize.y) //wide
			{
				return new Vector2(1f, ratioSize.y / ratioSize.x);
			}
			else if(ratioSize.x < ratioSize.y) //tall
			{
				return new Vector2(ratioSize.x / ratioSize.y, 1f);
			}
			else //square
			{
				return new Vector3(1f,1f);
			}
		}
	}
	*/

    private Vector3 _topLeftVert;
	public Vector3 TopLeftVert
    { 
        //return position in local space
		get
        {
			if(isQuad)
			{
				return RelativePos2(quadData.TopLeftVert);
			}
			else
			{
				_topLeftVert.x = chMinX;
				_topLeftVert.y = chMaxY;
				_topLeftVert.z = 0f;
				return RelativePos(_topLeftVert); 
			}
		}
	}

    private Vector3 _topRightVert;
	public Vector3 TopRightVert
    { 
        //return position in local space
		get
        {
			if(isQuad)
			{
				return RelativePos2(quadData.TopRightVert);
			}
			else
			{
            	_topRightVert.x = chMaxX;
				_topRightVert.y = chMaxY;
				_topRightVert.z = 0f;
				return RelativePos(_topRightVert);
			}
		}
	}

    private Vector3 _bottomRightVert;
	public Vector3 BottomRightVert
    { 
        //return position in local space
		get
        {
			if(isQuad)
			{
				return RelativePos2(quadData.BottomRightVert);
			}
			else
			{
				_bottomRightVert.x = chMaxX;
				_bottomRightVert.y = chMinY;
				_bottomRightVert.z = 0f;
				return RelativePos(_bottomRightVert);
			}
           
		}
	}

    private Vector3 _bottomLeftVert;
	public Vector3 BottomLeftVert
    { 
        //return position in local space
		get
        {
			if(isQuad)
			{
				return RelativePos2(quadData.BottomLeftVert);
			}
			else
			{
				_bottomLeftVert.x = chMinX;
				_bottomLeftVert.y = chMinY;
				_bottomLeftVert.z = 0f;
				return RelativePos(_bottomLeftVert);
			}
		}
	}

    private Vector3 _middle;
    public Vector3 Middle
    {
	    get
	    {
		    /*
			if(isQuad)
			{
				return RelativePos2(quadData.Middle);
			}
			else
			{

				_middle.x = (chMinX + chMaxX) * 0.5f;
				_middle.y = (chMinY + chMaxY) * 0.5f;
				_middle.z = 0f;
			}
			*/
		    //2024-04-09 this new method accounts for linespacing:
		    _middle.x = Mathf.Lerp(TopLeftVert.x, BottomRightVert.x, 0.5f);
		    _middle.y = Mathf.Lerp(TopLeftVert.y, BottomLeftVert.y, 0.5f) + lineSpacing - 1f;
		    return _middle;
	    }
    }

    Vector3 RelativePos_ReturnVal = Vector3.zero;
    float RelativePos_Multiplier;
    public Vector3 RelativePos(Vector3 yeah)
    {
        // RV: This is the OH YEAH method. OH YEAH!!!!! I, for one, feel refreshed.
        RelativePos_Multiplier = (size.y / chSize); //ch.size is quality
		//Debug.Log("My multiplier: " + RelativePos_Multiplier + "the numerator was: " + size + " the denomonator was: " + chSize);
        RelativePos_ReturnVal.x = pos.x + offset.x + yeah.x * (size.x / chSize);
        RelativePos_ReturnVal.y = pos.y + offset.y + yeah.y * RelativePos_Multiplier;
        RelativePos_ReturnVal.z = pos.z + offset.z + yeah.z * RelativePos_Multiplier;

        return RelativePos_ReturnVal; 
	}

    private Vector3 RelativePos2_ReturnVal = Vector3.zero;
    public Vector3 RelativePos2(Vector3 yeah)
    {
        //for quads
        RelativePos2_ReturnVal.x = pos.x + offset.x + yeah.x * size.x;
        RelativePos2_ReturnVal.y = pos.y + offset.y + yeah.y * size.y;
        RelativePos2_ReturnVal.z = pos.z + offset.z + yeah.z;

        return RelativePos2_ReturnVal; //ch.size is quality

    }
	public float RelativeWidth{ //width of this letter
		get{
			if(isQuad)
			{
				return quadData.size.x * size.x;
			}
			else
			{
				return chMaxX * (size.x / chSize);
			}
		}
	}
	public Vector3 RelativeAdvance(float extraSpacing, float quality)
	{
		return Advance(extraSpacing, quality) + pos;
	}
	public Vector3 RelativeAdvance(float extraSpacing)
	{
		return RelativeAdvance(extraSpacing, chSize);
	}
    Vector3 Advance_ReturnVal = Vector3.zero;
	public Vector3 Advance(float extraSpacing, float myQuality)
    { 
        //for getting letter position and autowrap data
		if(quadData != null)
        {
            Advance_ReturnVal.x = ((quadData.size.x + quadData.advance) * size.x) + (extraSpacing * size.x / myQuality);
            Advance_ReturnVal.y = 0f;
            Advance_ReturnVal.z = 0f;
		}
        else
        {
            Advance_ReturnVal.x = (chAdvance + (extraSpacing * size.x)) * (size.x / myQuality);
            Advance_ReturnVal.y = 0f;
            Advance_ReturnVal.z = 0f;
		}

        return Advance_ReturnVal;

    }
	public Vector3 Advance(float extraSpacing)
	{
		return Advance(extraSpacing, chSize);
	}
	public STMTextInfo(){ //dont use this unless ur gonna override it
		this.ch = new CharacterInfo();
		this.pos = Vector3.zero;

		this.lineSpacing = 1f;

		this.isEndOfParagraph = false;
		this.offset.x = 0f;
		this.offset.y = 0f;
		this.offset.z = 0f;
		this.line = 0;
		this.rawIndex = 0;
		this.indent = 0;
		//this.colorData = ScriptableObject.CreateInstance<STMColorData>();
//		this.colorData.color = Color.white;
		this.size.x = 16f;
		this.size.y = 16f;
        //this.delayData = ScriptableObject.CreateInstance<STMDelayData>();
        this.ev.Clear();// = new List<string>(); // RV: Why make new ones here? Clearing should be fine...
		this.ev2.Clear();// = new List<string>(); // RV: Same here...
        this.readTime = -1f;
		this.unreadTime = -1f;
		this.quadIndex = -1;
		this.isQuad = false;

		this.submeshChange = false;
		this.invoked = false;
	}
	public STMTextInfo(SuperTextMesh stm){ //for setting "defaults"
		SetValues(stm);
	}
	public void SetValues(SuperTextMesh stm)
	{
		this.lineSpacing = stm.lineSpacing;
		this.isEndOfParagraph = false;
		this.ch.style = stm.style;
//reset these ones too because of caching changes:
		this.gradientData = null;
		this.colorData = null;
		this.textureData = null;

		this.delayData = null;
		this.waveData = null;
		this.jitterData = null;
		this.audioClipData = null;
		this.fontData = null;
		this.quadData = null;


		//this.colorData = ScriptableObject.CreateInstance<STMColorData>();
//		this.colorData.color = stm.color;
		this.offset.x = 0f;
		this.offset.y = 0f;
		this.offset.z = 0f;
		this.indent = 0;
		this.rawIndex = 0;
		this.size.x = stm.size;
		this.size.y = stm.size;

		this.ev.Clear();// 
		this.ev2.Clear();//

		this.alignment = stm.alignment;
		this.stopPreviousSound = stm.stopPreviousSound;
		this.pitchMode = stm.pitchMode;
		this.overridePitch = stm.overridePitch;
		this.minPitch = stm.minPitch;
		this.maxPitch = stm.maxPitch;
		this.speedReadPitch = stm.speedReadPitch;
		this.readDelay = stm.readDelay;
		if(this.drawAnimData != null)
		{
			if(this.drawAnimData.name != stm.drawAnimName)
			{
				this.drawAnimData = Resources.Load<STMDrawAnimData>("STMDrawAnims/" + stm.drawAnimName);
				if(this.drawAnimData == null)
				{
					STMDrawAnimData[] tmpDrawAnims = Resources.LoadAll<STMDrawAnimData>("STMDrawAnims");
					if(tmpDrawAnims.Length > 0)
					{
						this.drawAnimData = tmpDrawAnims[0]; //get first one
					}
				}
			}
			//else do nothing, already has a match!
		}
		else
		{
			this.drawAnimData = Resources.Load<STMDrawAnimData>("STMDrawAnims/" + stm.drawAnimName);
			if(this.drawAnimData == null) //still null?
			{
				STMDrawAnimData[] tmpDrawAnims = Resources.LoadAll<STMDrawAnimData>("STMDrawAnims");
				if(tmpDrawAnims.Length > 0)
				{
					this.drawAnimData = tmpDrawAnims[0]; //get first one
				}
			}
		}
		this.drawOrder = stm.drawOrder;
		this.quadIndex = -1;
		this.isQuad = false;


		this.materialData = null;
		this.soundClipData = null;
		this.quadIndex = -1;

		this.submeshChange = false;
		this.invoked = false;
		
		this.chSize = stm.quality;
	}

	public STMTextInfo(STMTextInfo clone, CharacterInfo ch) : this(clone){ //clone everything but character. used for auto hyphens
		this.ch = ch;
		this.quadData = null; //yeah or else it gets weird. it's gonna be a hyphen/space so whatever!!
		this.isQuad = false;
	}
	
	public STMTextInfo(STMTextInfo clone){
		SetValues(clone);
	}
	public void SetValues(STMTextInfo clone)
	{
		this.lineSpacing = clone.lineSpacing;
		this.isEndOfParagraph = clone.isEndOfParagraph;

		this.ch = clone.ch;
		this.pos = clone.pos;
		this.offset.x = clone.offset.x;
		this.offset.y = clone.offset.y;
		this.offset.z = clone.offset.z;
		this.line = clone.line;
		this.rawIndex = clone.rawIndex;
		this.indent = clone.indent;
		
		if(clone.ev.Count > 0) //this check saves a TON of time... but im sure there's an even better way? can it just be set directly...?
			this.ev = new List<string>(clone.ev);
		if(clone.ev2.Count > 0)
			this.ev2 = new List<string>(clone.ev2);
		
		//yeah... does this cause errors? events are just strings so i dont see why... hm
		//this.ev = clone.ev;
		//this.ev2 = clone.ev2;


		this.colorData = clone.colorData;
		//this.colorData = ScriptableObject.CreateInstance<STMColorData>();
//		this.colorData.color = clone.colorData.color;
		this.gradientData = clone.gradientData;
		this.textureData = clone.textureData;
		this.size = clone.size;
		//this.delayData = ScriptableObject.CreateInstance<STMDelayData>();
//		this.delayData.count = clone.delayData.count;
		this.delayData = clone.delayData;
		this.waveData = clone.waveData;
		this.jitterData = clone.jitterData;
		this.alignment = clone.alignment;

		this.readTime = clone.readTime;
		this.unreadTime = clone.unreadTime;
		this.drawAnimData = clone.drawAnimData;

		//this.clipsFolderName = clone.clipsFolderName;
		this.audioClipData = clone.audioClipData;
		this.stopPreviousSound = clone.stopPreviousSound;
		this.pitchMode = clone.pitchMode;
		this.overridePitch = clone.overridePitch;
		this.minPitch = clone.minPitch;
		this.maxPitch = clone.maxPitch;
		this.speedReadPitch = clone.speedReadPitch;
		this.readDelay = clone.readDelay;
		this.drawOrder = clone.drawOrder;

		//this.submeshNumber = clone.submeshNumber;
		this.fontData = clone.fontData;
		this.quadData = clone.quadData;
		this.isQuad = clone.isQuad;
		this.materialData = clone.materialData;
		this.soundClipData = clone.soundClipData;

		this.quadIndex = clone.quadIndex;

		this.submeshChange = clone.submeshChange;
		this.invoked = false;
		
		this.chSize = clone.chSize;
	}
}
/*
NEW TAGS TO ADD:
character spacing tag
line height tag? (work automatically with size)
midway center alignment?? oh boy... maybe make it a special tag that can only be used at the start?
*/

/*
[System.Serializable]
public class STMImageData : ScriptableObject{ //inline images
	public string name;
	public Texture[] frames;
	public int rate;
}
*/
