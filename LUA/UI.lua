local rotationOn = 0;

local button = CreateFrame("Button", nil, UIParent)
button:SetPoint("TOP", UIParent, "TOP", 0, 0)
button:SetWidth(100)
button:SetHeight(20)
button:SetText("Frozen: Off")
button:SetNormalFontObject("GameFontNormal")
font = button:GetNormalFontObject();
font:SetTextColor(1, 0, 0);
button:SetNormalFontObject(font);
button:SetHighlightFontObject(font);
button:SetFrameStrata("TOOLTIP");

local ntex = button:CreateTexture()
local htex = button:CreateTexture()
local ptex = button:CreateTexture()

ntex:SetColorTexture(0, 0, 0, alphaColor)
ntex:SetAllPoints()
button:SetNormalTexture(ntex)

htex:SetColorTexture(0, 0, 0, 0.5)
htex:SetAllPoints()
button:SetHighlightTexture(htex)

ptex:SetColorTexture(0, 0, 0, 0.5)
ptex:SetAllPoints()
button:SetPushedTexture(ptex)

button:RegisterForClicks("AnyUp")

function startStop()
	if rotationOn == 1 then				
		button:SetText("Frozen: Off")
		font:SetTextColor(1, 0, 0);	
		startStopFrame.t:SetColorTexture(0, 0, 0, alphaColor)
		startStopFrame.t:SetAllPoints(startStopFrame)	
		rotationOn = 0
	else		
		button:SetText("Frozen: On")						
		font:SetTextColor(0, 1, 0);		
		startStopFrame.t:SetColorTexture(1, 1, 1, alphaColor)
		startStopFrame.t:SetAllPoints(startStopFrame)		
		rotationOn = 1
	end
end

button:SetScript("OnClick", startStop)

-- License: Public Domain - HideOderHallBar
local b = OrderHallCommandBar
b:SetScript("OnShow", b.Hide)
b:Hide()

-- initial call for other addons, re-register events
b:RequestCategoryInfo()
b:RegisterEvent("GARRISON_TALENT_COMPLETE")
b:RegisterEvent("GARRISON_TALENT_UPDATE")
b:RegisterUnitEvent("UNIT_PHASE", "player")

-- prevent buffs jumping up/down
UIParent:UnregisterEvent("UNIT_AURA")

-- Hide the annoying quest tracker frame 
--ObjectiveTrackerFrame:Hide()