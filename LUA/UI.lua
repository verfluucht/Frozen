local rotationOn = 0;
local cooldownOn = 0;

local space = 5
local width = 120
local buttonWidth = width - (space * 2);
local buttonHeight = 20

local overlay = CreateFrame("frame", "Overlay", UIParent)
overlay:SetPoint("TOP", UIParent, "TOP", 0, 0)
overlay:SetSize(width, 55)
overlay:SetFrameStrata("TOOLTIP");
overlay.t = overlay:CreateTexture()
overlay.t:SetColorTexture(0, 0, 1, 0.5)
overlay.t:SetAllPoints(overlay)

local button1 = CreateFrame("Button", nil, overlay)
button1:SetPoint("TOP", overlay, "TOP", 0, -space)
button1:SetWidth(buttonWidth)
button1:SetHeight(buttonHeight)
button1:SetText("Frozen: Off")
font1 = CreateFont("one")
font1:SetFont("Fonts\\FRIZQT__.TTF", 12, "OUTLINE")
font1:SetTextColor(1, 0, 0);
button1:SetNormalFontObject(font1);
button1:SetHighlightFontObject(font1);
button1:SetFrameStrata("TOOLTIP");

local normalTexture1 = button1:CreateTexture()
local highlightTexture1 = button1:CreateTexture()
local pushedTexture1 = button1:CreateTexture()

normalTexture1:SetColorTexture(0, 0, 0, alphaColor)
normalTexture1:SetAllPoints()
button1:SetNormalTexture(normalTexture1)

highlightTexture1:SetColorTexture(0, 0, 0, 0.5)
highlightTexture1:SetAllPoints()
button1:SetHighlightTexture(highlightTexture1)

pushedTexture1:SetColorTexture(0, 0, 0, 0.5)
pushedTexture1:SetAllPoints()
button1:SetPushedTexture(pushedTexture1)

button1:RegisterForClicks("AnyUp")

function startStop()
	if rotationOn == 1 then				
		button1:SetText("Frozen: Off")
		font1:SetTextColor(1, 0, 0);			
		rotationOn = 0
	else		
		button1:SetText("Frozen: On")						
		font1:SetTextColor(0, 1, 0);		
		rotationOn = 1
	end
	startStopFrame.t:SetColorTexture(rotationOn, cooldownOn, 0, alphaColor)
	startStopFrame.t:SetAllPoints(startStopFrame)		
end

button1:SetScript("OnClick", startStop)

----------------------------------------------------------------------------------------------------------------
------------------------- Cooldowns ----------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------

local button2 = CreateFrame("Button", nil, overlay)
button2:SetPoint("TOP", overlay, "TOP", 0, (-space * 2) - buttonHeight)
button2:SetWidth(buttonWidth)
button2:SetHeight(buttonHeight)
button2:SetText("Cooldowns: Off")
font2 = CreateFont("two")
font2:SetFont("Fonts\\FRIZQT__.TTF", 12, "OUTLINE")
font2:SetTextColor(1, 0, 0);
button2:SetNormalFontObject(font2);
button2:SetHighlightFontObject(font2);
button2:SetFrameStrata("TOOLTIP");

local normalTexture2 = button2:CreateTexture()
local highlightTexture2 = button2:CreateTexture()
local pushedTexture2 = button2:CreateTexture()

normalTexture2:SetColorTexture(0, 0, 0, alphaColor)
normalTexture2:SetAllPoints()
button2:SetNormalTexture(normalTexture2)

highlightTexture2:SetColorTexture(0, 0, 0, 0.5)
highlightTexture2:SetAllPoints()
button2:SetHighlightTexture(highlightTexture2)

pushedTexture2:SetColorTexture(0, 0, 0, 0.5)
pushedTexture2:SetAllPoints()
button2:SetPushedTexture(pushedTexture2)

button2:RegisterForClicks("AnyUp")

function cooldownOnOff()
	if cooldownOn == 1 then				
		button2:SetText("Cooldowns: Off")		
		font2:SetTextColor(1, 0, 0);		
		cooldownOn = 0
	else		
		button2:SetText("Cooldowns: On")	
		font2:SetTextColor(0, 1, 0);		
		cooldownOn = 1
	end	
	startStopFrame.t:SetColorTexture(rotationOn, cooldownOn, 0, alphaColor)
	startStopFrame.t:SetAllPoints(startStopFrame)	
end

button2:SetScript("OnClick", cooldownOnOff)