local size = 1

local index = GetCurrentResolution();
local currentResolution = select(index, GetScreenResolutions());  -- this will return a value in the format 1920x1080
local resWidth, resHeight = strsplit("x", currentResolution, 2)
local scale = GetScreenWidth() / resWidth 

local parent = CreateFrame("frame", "Recount", UIParent)
parent:SetSize(55 * size, 20 * size);  -- Width, Height
parent:SetPoint("TOPLEFT", 0, 0)
parent:SetScale(scale)
parent:RegisterEvent("ADDON_LOADED")
parent.t = parent:CreateTexture()
parent.t:SetColorTexture(0, 0, 0, 1)
parent.t:SetAllPoints(parent)
parent:SetFrameStrata("TOOLTIP");

local function InitializeOne()
    local i = 0

	print("Width: " .. GetScreenWidth() .. "/Real Width: " .. resWidth .. " = Scale:" .. scale)

	--print ("Initialising Health Frames")	
	healthFrame = CreateFrame("frame", "", parent)	
	healthFrame:SetSize(size, size)
	healthFrame:SetPoint("TOPLEFT", 0, 0)                         -- row 1, column 1 [RED]
	healthFrame.t = healthFrame:CreateTexture()        
	healthFrame.t:SetColorTexture(1, 0, 0, alphaColor)
	healthFrame.t:SetAllPoints(healthFrame)	
	healthFrame:Show()
    
	--print ("Initialising Power Frames (Rage, Energy, etc...)")  
	powerFrame = CreateFrame("frame","", parent)
	powerFrame:SetSize(size, size)
	powerFrame:SetPoint("TOPLEFT", 1 * size, 0)                   -- row 1, column 2 [GREEN]
	powerFrame.t = powerFrame:CreateTexture()        
	powerFrame.t:SetColorTexture(0, 1, 0, alphaColor)
	powerFrame.t:SetAllPoints(powerFrame)    
	powerFrame:Show()

	--print ("Initialising Target Health Frames")    
	targetHealthFrame = CreateFrame("frame","", parent)
	targetHealthFrame:SetSize(size, size)
	targetHealthFrame:SetPoint("TOPLEFT", 2 * size, 0)            -- row 1, column 3 [BLUE]
	targetHealthFrame.t = targetHealthFrame:CreateTexture()        
	targetHealthFrame.t:SetColorTexture(0, 0, 1, alphaColor)
	targetHealthFrame.t:SetAllPoints(targetHealthFrame)
	targetHealthFrame:Show()
 	
    --print ("Initialising InCombat Frames")    
    unitCombatFrame = CreateFrame("frame","", parent);
    unitCombatFrame:SetSize(size, size);
    unitCombatFrame:SetPoint("TOPLEFT", 3 * size, 0)              -- row 1, column 4 [RED]
    unitCombatFrame.t = unitCombatFrame:CreateTexture()
    unitCombatFrame.t:SetColorTexture(1, 0, 0, alphaColor)
    unitCombatFrame.t:SetAllPoints(unitCombatFrame)
    unitCombatFrame:Show()

    --print ("Initialising UnitPower Frames")    
	unitPowerFrame = CreateFrame("frame","", parent);
	unitPowerFrame:SetSize(size, size)
	unitPowerFrame:SetPoint("TOPLEFT", 4 * size, 0)               -- row 1, column 5 [GREEN]
	unitPowerFrame.t = unitPowerFrame:CreateTexture()        
	unitPowerFrame.t:SetColorTexture(0, 1, 0, alphaColor)
	unitPowerFrame.t:SetAllPoints(unitPowerFrame)
	unitPowerFrame:Show()
	
    --print ("Initialising IsTargetFriendly Frame")
    isTargetFriendlyFrame = CreateFrame("frame","", parent);
    isTargetFriendlyFrame:SetSize(size, size);
    isTargetFriendlyFrame:SetPoint("TOPLEFT", 5 * size, 0)     -- row 1, column 6 [BLUE]
    isTargetFriendlyFrame.t = isTargetFriendlyFrame:CreateTexture()        
    isTargetFriendlyFrame.t:SetColorTexture(0, 0, 1, alphaColor)
    isTargetFriendlyFrame.t:SetAllPoints(isTargetFriendlyFrame)
    isTargetFriendlyFrame:Show()

	--print ("Initialising HasTarget Frame")
	hasTargetFrame = CreateFrame("frame","", parent);
	hasTargetFrame:SetSize(size, size);
	hasTargetFrame:SetPoint("TOPLEFT", 6 * size, 0)           -- row 1, column 7 [RED]
	hasTargetFrame.t = hasTargetFrame:CreateTexture()        
	hasTargetFrame.t:SetColorTexture(1, 0, 0, alphaColor)
	hasTargetFrame.t:SetAllPoints(hasTargetFrame)
	hasTargetFrame:Show()				
	
	--print ("Initialising PlayerIsCasting Frame")
	playerIsCastingFrame = CreateFrame("frame","", parent);
	playerIsCastingFrame:SetSize(size, size);
	playerIsCastingFrame:SetPoint("TOPLEFT", 7 * size, 0)     -- row 1, column 8 [GREEN]
	playerIsCastingFrame.t = playerIsCastingFrame:CreateTexture()        
	playerIsCastingFrame.t:SetColorTexture(0, 1, 0, alphaColor)
	playerIsCastingFrame.t:SetAllPoints(playerIsCastingFrame)
	playerIsCastingFrame:Show()				
	
	--print ("Initialising TargetIsCasting Frame")
	targetIsCastingFrame = CreateFrame("frame","", parent);
	targetIsCastingFrame:SetSize(size, size);
	targetIsCastingFrame:SetPoint("TOPLEFT", 8 * size, 0)     -- row 1, column 9 [BLUE]
	targetIsCastingFrame.t = targetIsCastingFrame:CreateTexture()        
	targetIsCastingFrame.t:SetColorTexture(0, 0, 1, alphaColor)
	targetIsCastingFrame.t:SetAllPoints(targetIsCastingFrame)
	targetIsCastingFrame:Show()					
end

local function eventHandler(self, event, ...)
	local arg1 = ...
	if event == "ADDON_LOADED" then
		if (arg1 == "[PixelMagic]") then
			InitializeOne()
		end
	end
end	
parent:SetScript("OnEvent", eventHandler)

