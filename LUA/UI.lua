local rotationOn = 0;

local button = CreateFrame("Button", nil, UIParent)
button:SetPoint("TOP", UIParent, "TOP", 0, 0)
button:SetWidth(85)
button:SetHeight(20)
button:SetText("Rotation: Off")
button:SetNormalFontObject("GameFontNormal")
button:SetFrameStrata("TOOLTIP");

local ntex = button:CreateTexture()
ntex:SetTexture("Interface/Buttons/UI-Button-Outline")
ntex:SetTexCoord(0, 0, 0, 0)
ntex:SetAllPoints()
button:SetNormalTexture(ntex)

local htex = button:CreateTexture()
htex:SetTexture("Interface/Buttons/UI-Button-Borders2")
htex:SetTexCoord(0, 0, 0, 0)
htex:SetAllPoints()
button:SetHighlightTexture(htex)

local ptex = button:CreateTexture()
ptex:SetTexture("Interface/Buttons/UI-Panel-Button-Down")
ptex:SetTexCoord(0, 0, 0, 0)
ptex:SetAllPoints()
button:SetPushedTexture(ptex)

button:RegisterForClicks("AnyDown")

function startStop()
	if rotationOn == 1 then				
		button:SetText("Rotation: Off")		
		startStopFrame.t:SetColorTexture(0, 0, 0, alphaColor)
		startStopFrame.t:SetAllPoints(startStopFrame)	
		rotationOn = 0
	else		
		button:SetText("Rotation: On")				
		startStopFrame.t:SetColorTexture(1, 1, 1, alphaColor)
		startStopFrame.t:SetAllPoints(startStopFrame)		
		rotationOn = 1
	end
end

button:SetScript("OnClick", startStop)