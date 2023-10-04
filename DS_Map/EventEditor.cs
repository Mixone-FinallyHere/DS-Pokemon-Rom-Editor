using DSPRE.Resources;
using DSPRE.ROMFiles;
using LibNDSFormats.NSBMD;
using NSMBe4.NSBMD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static DSPRE.RomInfo;

namespace DSPRE {

    public partial class EventEditor : UserControl {
        private MainProgram parent;

        public EventEditor() {
            InitializeComponent();
            //parent = (this.Parent as MainProgram);
        }

        public void SetupEventEditor(MainProgram main) {
            parent = main;
            /* Extract essential NARCs sub-archives*/

            parent.statusLabelMessage("Attempting to unpack Event Editor NARCs... Please wait. This might take a while");

            DSUtils.TryUnpackNarcs(new List<DirNames> { DirNames.matrices,
                DirNames.maps,
                DirNames.exteriorBuildingModels,
                DirNames.buildingConfigFiles,
                DirNames.buildingTextures,
                DirNames.mapTextures,
                DirNames.areaData,

                DirNames.eventFiles,
                DirNames.trainerProperties,
                DirNames.OWSprites,

                DirNames.scripts,
            });

            RomInfo.SetOWtable();
            RomInfo.Set3DOverworldsDict();

            if (RomInfo.gameFamily == gFamEnum.HGSS) {
                DSUtils.TryUnpackNarcs(new List<DirNames> { DirNames.interiorBuildingModels });
            }

            parent.disableHandlers = true;
            if (File.Exists(RomInfo.OWtablePath)) {
                switch (RomInfo.gameFamily) {
                    case gFamEnum.DP:
                    case gFamEnum.Plat:
                        break;

                    default:
                        // HGSS Overlay 1 must be decompressed in order to read the overworld table
                        if (DSUtils.CheckOverlayHasCompressionFlag(1)) {
                            if (DSUtils.OverlayIsCompressed(1)) {
                                if (DSUtils.DecompressOverlay(1) < 0) {
                                    MessageBox.Show("Overlay 1 couldn't be decompressed.\nOverworld sprites in the Event Editor will be " +
                                "displayed incorrectly or not displayed at all.", "Unexpected EOF", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }

                        break;
                }
            }

            /* Add event file numbers to box */
            parent.statusLabelMessage("Loading Events... Please wait.");
            Update();

            int eventCount = RomInfo.GetEventFileCount();
            RomInfo.ReadOWTable();

            eventEditorWarpHeaderListBox.Items.Clear();
            eventEditorWarpHeaderListBox.Items.AddRange(parent.headerListBoxNames.ToArray());
            eventEditorHeaderLocationNameLabel.Text = "";

            string[] trainerNames = parent.GetTrainerNames();
            parent.StartProgress(0, (int)(eventCount + RomInfo.OverworldTable.Keys.Max() + trainerNames.Length));

            /* Add event list to event combobox */
            selectEventComboBox.Items.Clear();
            for (int i = 0; i < eventCount; i++) {
                selectEventComboBox.Items.Add("Event File " + i);
                parent.IncreaseProgress();
            }

            /* Add sprite list to ow sprite box */
            owSpriteComboBox.Items.Clear();
            foreach (ushort key in RomInfo.OverworldTable.Keys) {
                owSpriteComboBox.Items.Add("OW Entry " + key);
                parent.IncreaseProgress();
            }

            /* Add trainer list to ow trainer box */
            owTrainerComboBox.Items.Clear();
            owTrainerComboBox.Items.AddRange(trainerNames);

            /* Add item list to ow item box */
            string[] itemNames = RomInfo.GetItemNames();
            if (ROMToolboxDialog.CheckScriptsStandardizedItemNumbers()) {
                UpdateItemComboBox(itemNames);
            } else {
                ScriptFile itemScript = new ScriptFile(RomInfo.itemScriptFileNumber);
                owItemComboBox.Items.Clear();
                foreach (CommandContainer cont in itemScript.allScripts) {
                    if (cont.commands.Count > 4) {
                        continue;
                    }
                    owItemComboBox.Items.Add(BitConverter.ToUInt16(cont.commands[1].cmdParams[1], 0) + "x " + itemNames[BitConverter.ToUInt16(cont.commands[0].cmdParams[1], 0)]);
                }
            }

            /* Add ow movement list to box */
            owMovementComboBox.Items.Clear();
            spawnableDirComboBox.Items.Clear();
            spawnableTypeComboBox.Items.Clear();
            owMovementComboBox.Items.AddRange(PokeDatabase.EventEditor.Overworlds.movementsArray);
            spawnableDirComboBox.Items.AddRange(PokeDatabase.EventEditor.Spawnables.orientationsArray);
            spawnableTypeComboBox.Items.AddRange(PokeDatabase.EventEditor.Spawnables.typesArray);

            /* Draw matrix 0 in matrix navigator */
            eventMatrix = new GameMatrix(0);

            showSpawnablesCheckBox.Checked = Properties.Settings.Default.renderSpawnables;
            showOwsCheckBox.Checked = Properties.Settings.Default.renderOverworlds;
            showWarpsCheckBox.Checked = Properties.Settings.Default.renderWarps;
            showTriggersCheckBox.Checked = Properties.Settings.Default.renderTriggers;

            if (owOrientationComboBox.SelectedIndex < 0 && overworldsListBox.Items.Count <= 0) {
                owOrientationComboBox.SelectedIndex = 2;
            }

            if (owMovementComboBox.SelectedIndex < 0 && overworldsListBox.Items.Count <= 0) {
                owOrientationComboBox.SelectedIndex = 1;
            }

            eventMatrixUpDown.Maximum = parent.GetRomInfo().GetMatrixCount() - 1;
            eventAreaDataUpDown.Maximum = parent.GetRomInfo().GetAreaDataCount() - 1;

            parent.disableHandlers = false;

            selectEventComboBox.SelectedIndex = 0;
            owItemComboBox.SelectedIndex = 0;
            owTrainerComboBox.SelectedIndex = 0;

            parent.FinishProgress();

            parent.statusLabelMessage();
        }

        #region Variables

        public const byte tileSize = 16;
        public static NSBMDGlRenderer eventBuildingsRenderer = new NSBMDGlRenderer();
        public static MapFile eventMapFile;
        public static NSBMDGlRenderer eventMapRenderer = new NSBMDGlRenderer();
        public EventFile currentEvFile;
        public Brush eventBrush;
        public GameMatrix eventMatrix;
        public Rectangle eventMatrixRectangle;
        public Pen eventPen;
        public NSMBe4.NSBMD.NSBTX_File overworldFrames;
        public Event selectedEvent;
        private bool itemComboboxIsUpToDate = false;
        /* Painters to draw the matrix grid */

        #endregion Variables

        #region Subroutines

        public string GetCurrentEventFileName() {
            return selectEventComboBox.SelectedIndex.ToString("D4");
        }

        public void InitOpenGL() {
            eventOpenGlControl.InitializeContexts();
        }

        public void Modify_eventAreaUpDown_Max(bool increase) {
            if (increase) {
                eventAreaDataUpDown.Maximum++;
            } else {
                eventAreaDataUpDown.Maximum--;
            }
        }

        public void Modify_eventMatrixUpDown_Max(bool increase) {
            if (increase) {
                eventMatrixUpDown.Maximum++;
            } else {
                eventMatrixUpDown.Maximum--;
            }
        }
        public void ObtainOpenGL() {
            eventOpenGlControl.MakeCurrent();
        }

        public void StopOpenGL() {
            eventOpenGlControl.Invalidate();
        }
        public void TriggerEventChange() {
            selectEventComboBox_SelectedIndexChanged(null, null);
        }

        public void UpdateFromHeaderTab(decimal matrix, int eventID, decimal area) {
            if (matrix != 0) {
                eventAreaDataUpDown.Value = area; // Use Area Data for textures if matrix is not 0
            }

            eventMatrixUpDown.Value = matrix; // Open the right matrix in event editor
            selectEventComboBox.SelectedIndex = eventID; // Select event file

            eventMatrixUpDown_ValueChanged(null, null);
        }

        public void UpdateHeaderWarpName(int index, string name) {
            eventEditorWarpHeaderListBox.Items[index] = name;
        }

        public void UpdateItemComboBox(string[] itemNames) {
            if (itemComboboxIsUpToDate) {
                return;
            }
            itemsSelectorHelpBtn.Visible = false;
            owItemComboBox.Size = new Size(new Point(owItemComboBox.Size.Width + 30, owItemComboBox.Size.Height));
            owItemComboBox.Items.Clear();
            owItemComboBox.Items.AddRange(itemNames);
            OWTypeChanged(null, null);
            itemComboboxIsUpToDate = true;
        }

        public void UpdateTrainerName(int index, string name) {
            owTrainerComboBox.Items[index] = name;
        }
        private void CenterEventViewOnEntities() {
            int destX = 0;
            int destY = 0;
            if (currentEvFile.overworlds.Count > 0) {
                destX = currentEvFile.overworlds[0].xMatrixPosition;
                destY = currentEvFile.overworlds[0].yMatrixPosition;
            } else if (currentEvFile.warps.Count > 0) {
                destX = currentEvFile.warps[0].xMatrixPosition;
                destY = currentEvFile.warps[0].yMatrixPosition;
            } else if (currentEvFile.spawnables.Count > 0) {
                destX = currentEvFile.spawnables[0].xMatrixPosition;
                destY = currentEvFile.spawnables[0].yMatrixPosition;
            } else if (currentEvFile.triggers.Count > 0) {
                destX = currentEvFile.triggers[0].xMatrixPosition;
                destY = currentEvFile.triggers[0].yMatrixPosition;
            }

            if (destX > eventMatrixXUpDown.Maximum || destY > eventMatrixYUpDown.Maximum) {
                //MessageBox.Show("One of the events tried to reference a bigger Matrix.\nMake sure the Header File associated to this Event File is using the correct Matrix.", "Error",
                //        MessageBoxButtons.OK, MessageBoxIcon.Error);
                eventMatrixXUpDown.Value = eventMatrixYUpDown.Value = 0;
            } else {
                eventMatrixXUpDown.Value = destX;
                eventMatrixYUpDown.Value = destY;
            }
        }

        private void centerEventViewOnSelectedEvent_Click(object sender, EventArgs e) {
            if (selectedEvent is null) {
                MessageBox.Show("You haven't selected any event.", "Nothing to do here", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                try {
                    eventMatrixXUpDown.Value = selectedEvent.xMatrixPosition;
                    eventMatrixYUpDown.Value = selectedEvent.yMatrixPosition;
                } catch (ArgumentOutOfRangeException) {
                    DialogResult main = MessageBox.Show("The selected event tried to reference a bigger Matrix than the one which is currently being displayed.\nDo you want to check for another potentially compatible matrix?", "Event is out of range", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (main.Equals(DialogResult.Yes)) {
                        ushort[] result = HeaderSearch.AdvancedSearch(0, (ushort)parent.internalNames.Count, parent.internalNames, (int)MapHeader.SearchableFields.EventFileID, (int)HeaderSearch.NumOperators.Equal, selectEventComboBox.SelectedIndex.ToString())
                            .Select(x => ushort.Parse(x.Split()[0]))
                            .ToArray();

                        Dictionary<ushort, ushort> dict = new Dictionary<ushort, ushort>();

                        if (gameFamily.Equals(gFamEnum.DP)) {
                            foreach (ushort headerID in result) {
                                HeaderDP hdp = (HeaderDP)MapHeader.LoadFromARM9(headerID);

                                if (hdp.matrixID != eventMatrixUpDown.Value && hdp.locationName != 0) {
                                    dict.Add(hdp.ID, hdp.matrixID);
                                }
                            }
                        } else {
                            foreach (ushort headerID in result) {
                                HeaderPt hpt;

                                if (ROMToolboxDialog.flag_DynamicHeadersPatchApplied || ROMToolboxDialog.CheckFilesDynamicHeadersPatchApplied()) {
                                    hpt = (HeaderPt)MapHeader.LoadFromFile(RomInfo.gameDirs[DirNames.dynamicHeaders].unpackedDir + "\\" + headerID.ToString("D4"), headerID, 0);
                                } else {
                                    hpt = (HeaderPt)MapHeader.LoadFromARM9(headerID);
                                }

                                if (hpt.matrixID != eventMatrixUpDown.Value && hpt.locationName != 0) {
                                    dict.Add(hpt.ID, hpt.matrixID);
                                }
                            }
                        }

                        if (dict.Count < 1) {
                            MessageBox.Show("DSPRE could not find another Header referencing the same Event File and a different Matrix.", "Search failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            eventMatrixXUpDown.Value = 0;
                            eventMatrixYUpDown.Value = 0;
                        } else {
                            if (dict.Count > 1) {
                                if (dict.Keys.Contains(parent.currentHeader.ID)) {
                                    DialogResult yn = MessageBox.Show("DSPRE found multiple Headers referencing the same Event File and a different Matrix.\n" +
                                        $"The last selected Header ({parent.currentHeader.ID}) is one of those.\n" +
                                        $"Do you want to load its matrix (#{parent.currentHeader.matrixID}?)", "Potential solution found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                    if (yn.Equals(DialogResult.Yes)) {
                                        eventMatrixUpDown.Value = parent.currentHeader.matrixID;
                                    }
                                } else {
                                    var kvp = dict.First();

                                    DialogResult yn = MessageBox.Show($"DSPRE found {dict.Count} Headers referencing the same Event File and a different Matrix.\n" +
                                        $"Do you want to load Header {kvp.Key}'s matrix (#{kvp.Value})?", "Potential solution found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                    if (yn.Equals(DialogResult.Yes)) {
                                        eventMatrixUpDown.Value = kvp.Value;
                                    }
                                }
                            } else {
                                var kvp = dict.First();

                                DialogResult yn = MessageBox.Show($"Header {kvp.Key}'s matrix (#{kvp.Value}) seems to be the only match for this Event File.\n" +
                                        $"Do you want to load it?", "Potential solution found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (yn.Equals(DialogResult.Yes)) {
                                    eventMatrixUpDown.Value = kvp.Value;
                                }
                            }
                        }
                    }
                } finally {
                    Update();
                }
            }
        }

        private void DisplayActiveEvents() {
            eventPictureBox.Image = new Bitmap(eventPictureBox.Width, eventPictureBox.Height);

            using (Graphics g = Graphics.FromImage(eventPictureBox.Image)) {
                Bitmap icon;

                /* Draw spawnables */
                if (showSpawnablesCheckBox.Checked) {
                    icon = (Bitmap)Properties.Resources.ResourceManager.GetObject("sign");

                    for (int i = 0; i < currentEvFile.spawnables.Count; i++) {
                        Spawnable spawnable = currentEvFile.spawnables[i];

                        if (isEventOnCurrentMatrix(spawnable)) {
                            g.DrawImage(icon, spawnable.xMapPosition * (tileSize + 1), spawnable.yMapPosition * (tileSize + 1));
                            if (selectedEvent == spawnable) { // Draw selection rectangle if event is the selected one
                                DrawSelectionRectangle(g, spawnable);
                            }
                        }
                    }
                }

                /* Draw overworlds */
                if (showOwsCheckBox.Checked) {
                    for (int i = 0; i < currentEvFile.overworlds.Count; i++) {
                        Overworld overworld = currentEvFile.overworlds[i];

                        if (isEventOnCurrentMatrix(overworld)) { // Draw image only if event is in current map
                            Bitmap sprite = GetOverworldImage(overworld.overlayTableEntry, overworld.orientation);
                            sprite.MakeTransparent();
                            g.DrawImage(sprite, (overworld.xMapPosition) * (tileSize + 1) - 7 + (32 - sprite.Width) / 2, (overworld.yMapPosition - 1) * (tileSize + 1) + (32 - sprite.Height));

                            if (selectedEvent == overworld) {
                                DrawSelectionRectangleOverworld(g, overworld);
                            }
                        }
                    }
                }

                /* Draw warps */
                if (showWarpsCheckBox.Checked) {
                    DrawWarpCollisions(g);

                    icon = (Bitmap)Properties.Resources.ResourceManager.GetObject("warp");
                    for (int i = 0; i < currentEvFile.warps.Count; i++) {
                        Warp warp = currentEvFile.warps[i];

                        if (isEventOnCurrentMatrix(warp)) {
                            g.DrawImage(icon, warp.xMapPosition * (tileSize + 1), warp.yMapPosition * (tileSize + 1));

                            if (selectedEvent == warp) { // Draw selection rectangle if event is the selected one
                                DrawSelectionRectangle(g, warp);
                            }
                        }
                    }
                }

                /* Draw triggers */
                if (showTriggersCheckBox.Checked) {
                    icon = (Bitmap)Properties.Resources.ResourceManager.GetObject("trigger");

                    for (int i = 0; i < currentEvFile.triggers.Count; i++) {
                        Trigger trigger = currentEvFile.triggers[i];

                        if (isEventOnCurrentMatrix(trigger)) {
                            for (int y = 0; y < currentEvFile.triggers[i].heightY; y++) {
                                for (int x = 0; x < currentEvFile.triggers[i].widthX; x++) {
                                    g.DrawImage(icon, (trigger.xMapPosition + x) * (tileSize + 1), (trigger.yMapPosition + y) * (tileSize + 1));
                                }
                            }
                            if (selectedEvent == trigger) {// Draw selection rectangle if event is the selected one
                                DrawSelectionRectangleTrigger(g, trigger);
                            }
                        }
                    }
                }
            }

            eventPictureBox.Invalidate();
        }

        private void DisplayEventMap(bool readGraphicsFromHeader = true) {
            /* Determine map file to open and open it in BinaryReader, unless map is VOID */
            uint mapIndex = GameMatrix.EMPTY;
            if (eventMatrixXUpDown.Value > eventMatrix.width || eventMatrixYUpDown.Value > eventMatrix.height) {
                String errorMsg = "This event file contains elements located on an unreachable map, beyond the current matrix.\n" +
                    "It is strongly advised that you bring every Overworld, Spawnable, Warp and Trigger of this event to a map that belongs to the matrix's range.";
                MessageBox.Show(errorMsg, "Can't load proper map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } else {
                mapIndex = eventMatrix.maps[(int)(eventMatrixYUpDown.Value), (int)(eventMatrixXUpDown.Value)];
            }

            if (mapIndex == GameMatrix.EMPTY) {
                eventPictureBox.BackgroundImage = new Bitmap(eventPictureBox.Width, eventPictureBox.Height);
                using (Graphics g = Graphics.FromImage(eventPictureBox.BackgroundImage)) {
                    g.Clear(Color.Black);
                }
                eventMapFile = null;
            } else {
                /* Determine area data */
                byte areaDataID;
                if (eventMatrix.hasHeadersSection && readGraphicsFromHeader) {
                    ushort headerID = (ushort)eventMatrix.headers[(short)eventMatrixYUpDown.Value, (short)eventMatrixXUpDown.Value];
                    MapHeader h;
                    if (ROMToolboxDialog.flag_DynamicHeadersPatchApplied || ROMToolboxDialog.CheckFilesDynamicHeadersPatchApplied()) {
                        h = MapHeader.LoadFromFile(RomInfo.gameDirs[DirNames.dynamicHeaders].unpackedDir + "\\" + headerID.ToString("D4"), headerID, 0);
                    } else {
                        h = MapHeader.LoadFromARM9(headerID);
                    }

                    areaDataID = h.areaDataID;

                    bool disableHandlersbackup = parent.disableHandlers;
                    parent.disableHandlers = true;
                    eventAreaDataUpDown.Value = h.areaDataID;
                    parent.disableHandlers = disableHandlersbackup;
                } else {
                    areaDataID = (byte)eventAreaDataUpDown.Value;
                }

                /* get texture file numbers from area data */
                AreaData areaData = new AreaData(areaDataID);

                /* Read map and building models, match them with textures and render them*/
                eventMapFile = new MapFile((int)mapIndex, RomInfo.gameFamily, discardMoveperms: false);
                parent.MW_LoadModelTextures(eventMapFile.mapModel, gameDirs[DirNames.mapTextures].unpackedDir, areaData.mapTileset);

                bool isInteriorMap = false;
                if ((RomInfo.gameVersion == gVerEnum.HeartGold || RomInfo.gameVersion == gVerEnum.SoulSilver) && areaData.areaType == 0x0)
                    isInteriorMap = true;

                for (int i = 0; i < eventMapFile.buildings.Count; i++) {
                    eventMapFile.buildings[i].LoadModelData(parent.GetRomInfo().GetBuildingModelsDirPath(isInteriorMap)); // Load building nsbmd
                    parent.MW_LoadModelTextures(eventMapFile.buildings[i].NSBMDFile, RomInfo.gameDirs[DirNames.buildingTextures].unpackedDir, areaData.buildingsTileset); // Load building textures
                }

                parent.RenderMap(ref eventMapRenderer, ref eventBuildingsRenderer, ref eventMapFile, 0f, 115.0f, 90f, 4f, eventOpenGlControl.Width, eventOpenGlControl.Height, true, true);
                eventPictureBox.BackgroundImage = parent.GrabMapScreenshot(eventOpenGlControl.Width, eventOpenGlControl.Height);
            }
            eventPictureBox.Invalidate();
        }

        private void DrawEventMatrix() {
            eventMatrixPictureBox.Image = new Bitmap(1 + 16 * eventMatrix.width, 1 + 16 * eventMatrix.height);

            using (Graphics g = Graphics.FromImage(eventMatrixPictureBox.Image)) {
                /* First, fill the rectangle with black */
                g.Clear(Color.Black);

                /* Now, draw the white cell borders on the black rectangle */
                eventPen = Pens.White;
                for (int y = 0; y < eventMatrix.height; y++) {
                    for (int x = 0; x < eventMatrix.width; x++) {
                        eventMatrixRectangle = new Rectangle(1 + 16 * x, 1 + 16 * y, 14, 14);
                        g.DrawRectangle(eventPen, eventMatrixRectangle);
                    }
                }
            }
        }

        private void DrawSelectionRectangle(Graphics g, Event ev) {
            eventPen = Pens.Red;
            g.DrawRectangle(eventPen, (ev.xMapPosition) * (tileSize + 1) - 1, (ev.yMapPosition) * (tileSize + 1) - 1, 18, 18);
            g.DrawRectangle(eventPen, (ev.xMapPosition) * (tileSize + 1) - 2, (ev.yMapPosition) * (tileSize + 1) - 2, 20, 20);
        }

        private void DrawSelectionRectangleOverworld(Graphics g, Overworld ow) {
            eventPen = Pens.Red;
            g.DrawRectangle(eventPen, (ow.xMapPosition) * (tileSize + 1) - 8, (ow.yMapPosition - 1) * (tileSize + 1), 34, 34);
            g.DrawRectangle(eventPen, (ow.xMapPosition) * (tileSize + 1) - 9, (ow.yMapPosition - 1) * (tileSize + 1) - 1, 36, 36);
        }

        private void DrawSelectionRectangleTrigger(Graphics g, Trigger t) {
            eventPen = Pens.Red;
            g.DrawRectangle(eventPen, (t.xMapPosition) * (tileSize + 1) - 1, (t.yMapPosition) * (tileSize + 1) - 1, 17 * t.widthX + 1, (tileSize + 1) * t.heightY + 1);
            g.DrawRectangle(eventPen, (t.xMapPosition) * (tileSize + 1) - 2, (t.yMapPosition) * (tileSize + 1) - 2, 17 * t.widthX + 3, (tileSize + 1) * t.heightY + 3);
        }

        private void DrawWarpCollisions(Graphics g) {
            if (eventMapFile != null) {
                Bitmap icon = (Bitmap)Properties.Resources.ResourceManager.GetObject("warpCollision");
                for (int y = 0; y < MapFile.mapSize; y++) {
                    for (int x = 0; x < MapFile.mapSize; x++) {
                        byte moveperm = eventMapFile.types[x, y];
                        if (PokeDatabase.System.MapCollisionTypePainters.TryGetValue(moveperm, out string val)) {
                            if (val.IndexOf("Warp", StringComparison.InvariantCultureIgnoreCase) >= 0) {
                                //Console.WriteLine("Found warp at " + i + ", " + j);
                                g.DrawImage(icon, y * (tileSize + 1), x * (tileSize + 1));
                            }
                        }
                    }
                }
            }
        }

        private void eventPictureBox_MouseMove(object sender, MouseEventArgs e) {
            Point coordinates = eventPictureBox.PointToClient(Cursor.Position);
            Point mouseTilePos = new Point(coordinates.X / (tileSize + 1), coordinates.Y / (tileSize + 1));
            parent.statusLabelMessage("Local: " + mouseTilePos.X + ", " + mouseTilePos.Y + "   |   " + "Global: " + (eventMatrixXUpDown.Value * MapFile.mapSize + mouseTilePos.X).ToString() + ", " + (eventMatrixYUpDown.Value * MapFile.mapSize + mouseTilePos.Y).ToString());
        }

        private void FillOverworldsBox() {
            overworldsListBox.Items.Clear();
            int count = currentEvFile.overworlds.Count;

            for (int i = 0; i < count; i++) {
                overworldsListBox.Items.Add(i.ToString("D" + Math.Max(0, count - 1).ToString().Length) + ": " + currentEvFile.overworlds[i].ToString());
            }
        }

        private void FillSpawnablesBox() {
            spawnablesListBox.Items.Clear();
            int count = currentEvFile.spawnables.Count;

            for (int i = 0; i < count; i++) {
                spawnablesListBox.Items.Add(i.ToString("D" + Math.Max(0, count - 1).ToString().Length) + ": " + currentEvFile.spawnables[i].ToString());
            }
        }

        private void FillTriggersBox() {
            triggersListBox.Items.Clear();
            int count = currentEvFile.triggers.Count;

            for (int i = 0; i < count; i++) {
                triggersListBox.Items.Add(i.ToString("D" + Math.Max(0, count - 1).ToString().Length) + ": " + currentEvFile.triggers[i].ToString());
            }
        }

        private void FillWarpsBox() {
            warpsListBox.Items.Clear();
            int count = currentEvFile.warps.Count;

            for (int i = 0; i < count; i++) {
                warpsListBox.Items.Add(i.ToString("D" + Math.Max(0, count - 1).ToString().Length) + ": " + currentEvFile.warps[i].ToString());
            }
        }

        private Bitmap GetOverworldImage(ushort eventEntryID, ushort orientation) {
            /* Find sprite corresponding to ID and load it*/
            if (RomInfo.ow3DSpriteDict.TryGetValue(eventEntryID, out string imageName)) { // If overworld is 3D, load image from dictionary
                return (Bitmap)Properties.Resources.ResourceManager.GetObject(imageName);
            }

            if (!RomInfo.OverworldTable.TryGetValue(eventEntryID, out (uint spriteID, ushort properties) result)) { // try loading image from dictionary
                return (Bitmap)Properties.Resources.ResourceManager.GetObject("overworld"); //if there's no match, load bounding box
            }

            try {
                FileStream stream = new FileStream(RomInfo.gameDirs[DirNames.OWSprites].unpackedDir + "\\" + result.spriteID.ToString("D4"), FileMode.Open);
                NSBTX_File nsbtx = new NSBTX_File(stream);

                if (nsbtx.texInfo.num_objs <= 1) {
                    return nsbtx.GetBitmap(0, 0).bmp; // Read nsbtx slot 0 if ow has only 2 frames
                }
                if (nsbtx.texInfo.num_objs <= 4) {
                    switch (orientation) {
                        case 0:
                            return nsbtx.GetBitmap(0, 0).bmp;

                        case 1:
                            return nsbtx.GetBitmap(1, 0).bmp;

                        case 2:
                            return nsbtx.GetBitmap(2, 0).bmp;

                        default:
                            return nsbtx.GetBitmap(3, 0).bmp;
                    }
                }
                if (nsbtx.texInfo.num_objs <= 8) { //Read nsbtx slot corresponding to overworld's movement
                    switch (orientation) {
                        case 0:
                            return nsbtx.GetBitmap(0, 0).bmp;

                        case 1:
                            return nsbtx.GetBitmap(2, 0).bmp;

                        case 2:
                            return nsbtx.GetBitmap(4, 0).bmp;

                        default:
                            return nsbtx.GetBitmap(6, 0).bmp;
                    }
                }
                if (nsbtx.texInfo.num_objs <= 16) { // Read nsbtx slot corresponding to overworld's movement
                    switch (orientation) {
                        case 0:
                            return nsbtx.GetBitmap(0, 0).bmp;

                        case 1:
                            return nsbtx.GetBitmap(11, 0).bmp;

                        case 2:
                            return nsbtx.GetBitmap(2, 0).bmp;

                        default:
                            return nsbtx.GetBitmap(4, 0).bmp;
                    }
                } else {
                    switch (orientation) {
                        case 0:
                            return nsbtx.GetBitmap(0, 0).bmp;

                        case 1:
                            return nsbtx.GetBitmap(27, 0).bmp;

                        case 2:
                            return nsbtx.GetBitmap(2, 0).bmp;

                        default:
                            return nsbtx.GetBitmap(4, 0).bmp;
                    }
                }
            } catch { // Load bounding box if sprite cannot be found
                return (Bitmap)Properties.Resources.ResourceManager.GetObject("overworldUnreadable");
            }
        }

        private bool isEventOnCurrentMatrix(Event ev) {
            if (ev.xMatrixPosition == eventMatrixXUpDown.Value) {
                if (ev.yMatrixPosition == eventMatrixYUpDown.Value) {
                    return true;
                }
            }

            return false;
        }

        private bool isEventUnderMouse(Event ev, Point mouseTilePos, int widthX = 0, int heightY = 0) {
            if (isEventOnCurrentMatrix(ev)) {
                Point evLocalCoords = new Point(ev.xMapPosition, ev.yMapPosition);
                Func<int, int, int, bool> checkRange = (mouseCoord, evCoord, extension) => mouseCoord >= evCoord && mouseCoord <= evCoord + extension;

                if (checkRange(mouseTilePos.X, evLocalCoords.X, widthX) && checkRange(mouseTilePos.Y, evLocalCoords.Y, heightY)) {
                    return true;
                }
            }
            return false;
        }

        private void MarkActiveCell(int xPosition, int yPosition) {
            /*  Redraw the matrix to avoid multiple green cells  */
            DrawEventMatrix();
            MarkUsedCells();

            /* Set rectangle to paint and brush color */
            eventMatrixRectangle = new Rectangle(2 + 16 * xPosition, 2 + 16 * yPosition, 13, 13);
            eventBrush = Brushes.Lime;

            /* Paint cell */
            using (Graphics g = Graphics.FromImage(eventMatrixPictureBox.Image)) {
                g.FillRectangle(eventBrush, eventMatrixRectangle);
            }

            /* Update PictureBox and current coordinates labels */
            eventMatrixPictureBox.Invalidate();
            eventMatrixXUpDown.Value = xPosition;
            eventMatrixYUpDown.Value = yPosition;
        }

        private void MarkUsedCells() {
            using (Graphics g = Graphics.FromImage(eventMatrixPictureBox.Image)) {
                eventBrush = Brushes.Orange;

                for (int i = 0; i < currentEvFile.spawnables.Count; i++) {
                    eventMatrixRectangle = new Rectangle(2 + 16 * currentEvFile.spawnables[i].xMatrixPosition, 2 + 16 * currentEvFile.spawnables[i].yMatrixPosition, 13, 13);
                    g.FillRectangle(eventBrush, eventMatrixRectangle);
                }
                for (int i = 0; i < currentEvFile.overworlds.Count; i++) {
                    eventMatrixRectangle = new Rectangle(2 + 16 * currentEvFile.overworlds[i].xMatrixPosition, 2 + 16 * currentEvFile.overworlds[i].yMatrixPosition, 13, 13);
                    g.FillRectangle(eventBrush, eventMatrixRectangle);
                }
                for (int i = 0; i < currentEvFile.warps.Count; i++) {
                    eventMatrixRectangle = new Rectangle(2 + 16 * currentEvFile.warps[i].xMatrixPosition, 2 + 16 * currentEvFile.warps[i].yMatrixPosition, 13, 13);
                    g.FillRectangle(eventBrush, eventMatrixRectangle);
                }
                for (int i = 0; i < currentEvFile.triggers.Count; i++) {
                    eventMatrixRectangle = new Rectangle(2 + 16 * currentEvFile.triggers[i].xMatrixPosition, 2 + 16 * currentEvFile.triggers[i].yMatrixPosition, 13, 13);
                    g.FillRectangle(eventBrush, eventMatrixRectangle);
                }
            }
            eventMatrixPictureBox.Invalidate();
        }
        private void updateSelectedOverworldName() {
            int index = overworldsListBox.SelectedIndex;
            if (index < 0) {
                return;
            }
            overworldsListBox.Items[index] = index.ToString("D" + Math.Max(0, overworldsListBox.Items.Count - 1).ToString().Length) + ": " + (selectedEvent as Overworld).ToString();
        }

        private void updateSelectedSpawnableName() {
            int index = spawnablesListBox.SelectedIndex;
            if (index < 0) {
                return;
            }
            spawnablesListBox.Items[index] = index.ToString("D" + Math.Max(0, spawnablesListBox.Items.Count - 1).ToString().Length) + ": " + (selectedEvent as Spawnable).ToString();
        }

        private void updateSelectedTriggerName() {
            int index = triggersListBox.SelectedIndex;
            if (index < 0) {
                return;
            }
            triggersListBox.Items[index] = index.ToString("D" + Math.Max(0, triggersListBox.Items.Count - 1).ToString().Length) + ": " + (selectedEvent as Trigger).ToString();
        }

        private void updateSelectedWarpName() {
            int index = warpsListBox.SelectedIndex;
            if (index < 0) {
                return;
            }
            warpsListBox.Items[index] = index.ToString("D" + Math.Max(0, warpsListBox.Items.Count - 1).ToString().Length) + ": " + (selectedEvent as Warp).ToString();
        }

        #endregion Subroutines

        #region EventHandlers

        private void addEventFileButton_Click(object sender, EventArgs e) {
            /* Add copy of event 0 to event folder */
            new EventFile().SaveToFileDefaultDir(selectEventComboBox.Items.Count);

            /* Update ComboBox and select new file */
            selectEventComboBox.Items.Add("Event File " + selectEventComboBox.Items.Count);
            selectEventComboBox.SelectedIndex = selectEventComboBox.Items.Count - 1;
        }

        private void eventAreaDataUpDown_ValueChanged(object sender, EventArgs e) {
            DisplayEventMap(readGraphicsFromHeader: false);
        }

        private void eventEditorFullMapReload(int coordX, int coordY) {
            /* Draw matrix image in the navigator */
            MarkActiveCell(coordX, coordY);
            /* Render events on map */
            DisplayEventMap();
            DisplayActiveEvents();
            eventMatrixPictureBox.Invalidate();
        }

        private void eventMatrixCoordsUpDown_ValueChanged(object sender, EventArgs e) {
            if (parent.disableHandlers) {
                return;
            }

            eventEditorFullMapReload((int)eventMatrixXUpDown.Value, (int)eventMatrixYUpDown.Value);
        }

        private void eventMatrixPictureBox_Click(object sender, EventArgs e) {
            const int squareSize = 16;
            Point coordinates = eventMatrixPictureBox.PointToClient(Cursor.Position);
            Point mouseTilePos = new Point(coordinates.X / squareSize, coordinates.Y / squareSize);

            MarkActiveCell(mouseTilePos.X, mouseTilePos.Y);
            eventMatrixXUpDown.Value = mouseTilePos.X;
            eventMatrixYUpDown.Value = mouseTilePos.Y;
        }

        private void eventMatrixUpDown_ValueChanged(object sender, EventArgs e) {
            if (parent.disableHandlers) {
                return;
            }
            parent.disableHandlers = true;

            eventMatrix = new GameMatrix((int)eventMatrixUpDown.Value);
            eventMatrixXUpDown.Maximum = eventMatrix.width - 1;
            eventMatrixYUpDown.Maximum = eventMatrix.height - 1;
            eventEditorFullMapReload((int)eventMatrixXUpDown.Value, (int)eventMatrixYUpDown.Value);

            parent.disableHandlers = false;

            CenterEventViewOnEntities();
        }

        private void eventPictureBox_Click(object sender, EventArgs e) {
            Point coordinates = eventPictureBox.PointToClient(Cursor.Position);
            Point mouseTilePos = new Point(coordinates.X / (tileSize + 1), coordinates.Y / (tileSize + 1));
            MouseEventArgs mea = (MouseEventArgs)e;

            if (mea.Button == MouseButtons.Left) {
                if (selectedEvent != null) {
                    switch (selectedEvent.evType) {
                        case Event.EventType.Spawnable:
                            if (!showSpawnablesCheckBox.Checked) {
                                return;
                            }
                            spawnableMapXUpDown.Value = (short)mouseTilePos.X;
                            spawnableMapYUpDown.Value = (short)mouseTilePos.Y;
                            spawnableXMatrixUpDown.Value = (short)eventMatrixXUpDown.Value;
                            spawnableYMatrixUpDown.Value = (short)eventMatrixYUpDown.Value;

                            break;

                        case Event.EventType.Overworld:
                            if (!showOwsCheckBox.Checked) {
                                return;
                            }
                            owXMapUpDown.Value = (short)mouseTilePos.X;
                            owYMapUpDown.Value = (short)mouseTilePos.Y;
                            owXMatrixUpDown.Value = (short)eventMatrixXUpDown.Value;
                            owYMatrixUpDown.Value = (short)eventMatrixYUpDown.Value;

                            break;

                        case Event.EventType.Warp:
                            if (!showWarpsCheckBox.Checked) {
                                return;
                            }
                            warpXMapUpDown.Value = (short)mouseTilePos.X;
                            warpYMapUpDown.Value = (short)mouseTilePos.Y;
                            warpXMatrixUpDown.Value = (short)eventMatrixXUpDown.Value;
                            warpYMatrixUpDown.Value = (short)eventMatrixYUpDown.Value;

                            break;

                        case Event.EventType.Trigger:
                            if (!showTriggersCheckBox.Checked) {
                                return;
                            }
                            triggerXMapUpDown.Value = (short)mouseTilePos.X;
                            triggerYMapUpDown.Value = (short)mouseTilePos.Y;
                            triggerXMatrixUpDown.Value = (short)eventMatrixXUpDown.Value;
                            triggerYMatrixUpDown.Value = (short)eventMatrixYUpDown.Value;

                            break;
                    }
                    DisplayActiveEvents();
                }
            } else if (mea.Button == MouseButtons.Right) {
                if (showWarpsCheckBox.Checked)
                    for (int i = 0; i < currentEvFile.warps.Count; i++) {
                        Warp ev = currentEvFile.warps[i];

                        if (isEventUnderMouse(ev, mouseTilePos)) {
                            if (ev == selectedEvent) {
                                goToWarpDestination_Click(sender, e);
                                return;
                            }
                            selectedEvent = ev;
                            eventsTabControl.SelectedTab = warpsTabPage;
                            warpsListBox.SelectedIndex = i;
                            DisplayActiveEvents();
                            return;
                        }
                    }
                if (showSpawnablesCheckBox.Checked)
                    for (int i = 0; i < currentEvFile.spawnables.Count; i++) {
                        Spawnable ev = currentEvFile.spawnables[i];

                        if (isEventUnderMouse(ev, mouseTilePos)) {
                            selectedEvent = ev;
                            eventsTabControl.SelectedTab = signsTabPage;
                            spawnablesListBox.SelectedIndex = i;
                            DisplayActiveEvents();
                            return;
                        }
                    }
                if (showOwsCheckBox.Checked)
                    for (int i = 0; i < currentEvFile.overworlds.Count; i++) {
                        Overworld ev = currentEvFile.overworlds[i];

                        if (isEventUnderMouse(ev, mouseTilePos)) {
                            selectedEvent = ev;
                            eventsTabControl.SelectedTab = overworldsTabPage;
                            overworldsListBox.SelectedIndex = i;
                            DisplayActiveEvents();
                            return;
                        }
                    }
                for (int i = 0; i < currentEvFile.triggers.Count; i++) {
                    Trigger ev = currentEvFile.triggers[i];

                    if (isEventUnderMouse(ev, mouseTilePos, ev.widthX - 1, ev.heightY - 1)) {
                        selectedEvent = ev;
                        eventsTabControl.SelectedTab = triggersTabPage;
                        triggersListBox.SelectedIndex = i;
                        DisplayActiveEvents();
                        return;
                    }
                }
            } else if (mea.Button == MouseButtons.Middle) {
                for (int i = 0; i < currentEvFile.warps.Count; i++) {
                    Warp ev = currentEvFile.warps[i];

                    if (isEventUnderMouse(ev, mouseTilePos)) {
                        if (ev == selectedEvent) {
                            goToWarpDestination_Click(sender, e);
                            return;
                        }
                    }
                }
            }
        }

        private void eventShiftDownButton_Click(object sender, EventArgs e) {
            if (eventMatrixYUpDown.Value < eventMatrix.height - 1) {
                eventMatrixYUpDown.Value += 1;
            }
        }

        private void eventShiftLeftButton_Click(object sender, EventArgs e) {
            if (eventMatrixXUpDown.Value > 0) {
                eventMatrixXUpDown.Value -= 1;
            }
        }

        private void eventShiftRightButton_Click(object sender, EventArgs e) {
            if (eventMatrixXUpDown.Value < eventMatrix.width - 1) {
                eventMatrixXUpDown.Value += 1;
            }
        }

        private void eventShiftUpButton_Click(object sender, EventArgs e) {
            if (eventMatrixYUpDown.Value > 0) {
                eventMatrixYUpDown.Value -= 1;
            }
        }

        private void eventsTabControl_SelectedIndexChanged(object sender, EventArgs e) {
            if (eventsTabControl.SelectedTab == signsTabPage) {
                int spawnablesCount = spawnablesListBox.Items.Count;

                if (spawnablesCount > 0) {
                    spawnablesListBox.SelectedIndex = 0;
                }
            } else if (eventsTabControl.SelectedTab == overworldsTabPage) {
                int overworldsCount = overworldsListBox.Items.Count;

                int selected = 0;

                if (overworldsCount > 0) {
                    if (selectedEvent is Overworld) {
                        int owId = (selectedEvent as Overworld).owID;
                        if (owId < overworldsCount) {
                            selected = owId;
                        }
                    }
                    overworldsListBox.SelectedIndex = selected;
                }
            } else if (eventsTabControl.SelectedTab == warpsTabPage) {
                int warpsCount = warpsListBox.Items.Count;

                if (warpsCount > 0) {
                    warpsListBox.SelectedIndex = 0;
                }
            } else if (eventsTabControl.SelectedTab == triggersTabPage) {
                int triggersCount = triggersListBox.Items.Count;

                if (triggersCount > 0) {
                    triggersListBox.SelectedIndex = 0;
                }
            }
        }

        private void exportEventFileButton_Click(object sender, EventArgs e) {
            currentEvFile.SaveToFileExplorePath("Event File " + selectEventComboBox.SelectedIndex);
        }

        private void importEventFileButton_Click(object sender, EventArgs e) {
            /* Prompt user to select .evt file */
            OpenFileDialog of = new OpenFileDialog {
                Filter = EventFile.DefaultFilter
            };
            if (of.ShowDialog(this) != DialogResult.OK) {
                return;
            }

            EventFile toImport = new EventFile(File.OpenRead(of.FileName));
            if (toImport.isEmpty()) {
                DialogResult d = MessageBox.Show("Are you sure you want to import an empty event file?\nAll existing entities in the current file will disappear.", "Empty File loaded", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (!d.Equals(DialogResult.Yes)) {
                    return;
                }

                /* Update event object on disk */
                File.Copy(of.FileName, RomInfo.gameDirs[DirNames.eventFiles].unpackedDir + "\\" + selectEventComboBox.SelectedIndex.ToString("D4"), true);

                /* Refresh controls */
                selectEventComboBox_SelectedIndexChanged(null, null);

                /* Display success message */
                MessageBox.Show("Events imported successfully!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                /* Refresh controls */
                selectEventComboBox_SelectedIndexChanged(null, null);
            } else {
                EventFileImport efi = new EventFileImport(toImport);
                efi.ShowDialog();

                if (!efi.DialogResult.Equals(DialogResult.OK)) {
                    return;
                }

                if (efi.blankCurrentEvents) {
                    DialogResult confirm = MessageBox.Show("You chose to import the entities into blank event file, which means all existing entities in the destination file will be deleted.\nProceed?", "Awaiting user confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (!confirm.Equals(DialogResult.Yes)) {
                        return;
                    }

                    currentEvFile = new EventFile();
                    spawnablesListBox.Items.Clear();
                    overworldsListBox.Items.Clear();
                    warpsListBox.Items.Clear();
                    triggersListBox.Items.Clear();
                }

                int[] currentArray;

                currentArray = efi.userSelected[(int)EventFile.serializationOrder.Spawnables];
                if (currentArray != null) {
                    foreach (int index in currentArray) {
                        currentEvFile.spawnables.Add(toImport.spawnables[index]);

                        spawnablesListBox.Items.Add("");
                        spawnablesListBox.SelectedIndex = spawnablesListBox.Items.Count - 1;
                        updateSelectedSpawnableName();
                    }
                }

                currentArray = efi.userSelected[(int)EventFile.serializationOrder.Overworlds];
                if (currentArray != null) {
                    foreach (int index in currentArray) {
                        currentEvFile.overworlds.Add(toImport.overworlds[index]);

                        overworldsListBox.Items.Add("");
                        overworldsListBox.SelectedIndex = overworldsListBox.Items.Count - 1;
                        updateSelectedOverworldName();
                    }
                }

                currentArray = efi.userSelected[(int)EventFile.serializationOrder.Warps];
                if (currentArray != null) {
                    foreach (int index in currentArray) {
                        currentEvFile.warps.Add(toImport.warps[index]);

                        warpsListBox.Items.Add("");
                        warpsListBox.SelectedIndex = warpsListBox.Items.Count - 1;
                        updateSelectedWarpName();
                    }
                }

                currentArray = efi.userSelected[(int)EventFile.serializationOrder.Triggers];
                if (currentArray != null) {
                    foreach (int index in currentArray) {
                        currentEvFile.triggers.Add(toImport.triggers[index]);

                        triggersListBox.Items.Add("");
                        triggersListBox.SelectedIndex = triggersListBox.Items.Count - 1;
                        updateSelectedTriggerName();
                    }
                }

                /* Display success message */
                MessageBox.Show("Events imported successfully!\nRemember to save the current Event File.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void itemsSelectorHelpBtn_Click(object sender, EventArgs e) {
            MessageBox.Show("This selector allows you to pick a preset Ground Item script from the game data.\n" +
                "Unlike in previous DSPRE versions, you can now change the Ground Item to be obtained even if you decided not to apply the Standardize Items patch from the Rom ToolBox.\n\n" +
                "However, some items are unavailable by default. The aforementioned patch can neutralize this limitation.\n\n",
                "About Ground Items", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void removeEventFileButton_Click(object sender, EventArgs e) {
            DialogResult d = MessageBox.Show("Are you sure you want to delete the last Event File?", "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (d.Equals(DialogResult.Yes)) {
                /* Delete event file */
                File.Delete(RomInfo.gameDirs[DirNames.eventFiles].unpackedDir + "\\" + (selectEventComboBox.Items.Count - 1).ToString("D4"));

                /* Check if currently selected file is the last one, and in that case select the one before it */
                int lastIndex = selectEventComboBox.Items.Count - 1;
                if (selectEventComboBox.SelectedIndex == lastIndex) {
                    selectEventComboBox.SelectedIndex--;
                }

                /* Remove item from ComboBox */
                selectEventComboBox.Items.RemoveAt(lastIndex);
            }
        }

        private void saveEventsButton_Click(object sender, EventArgs e) {
            currentEvFile.SaveToFileDefaultDir(selectEventComboBox.SelectedIndex);
        }

        private void selectEventComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (parent.disableHandlers) {
                return;
            }
            parent.disableHandlers = true;

            /* Load events data into EventFile class instance */
            currentEvFile = new EventFile(selectEventComboBox.SelectedIndex);

            /* Update ListBoxes */
            FillSpawnablesBox();
            FillOverworldsBox();
            FillTriggersBox();
            FillWarpsBox();
            ushort mapHeader = parent.eventToHeader[(ushort)selectEventComboBox.SelectedIndex];
            MapHeader h;
            if (ROMToolboxDialog.flag_DynamicHeadersPatchApplied || ROMToolboxDialog.CheckFilesDynamicHeadersPatchApplied()) {
                h = MapHeader.LoadFromFile(RomInfo.gameDirs[DirNames.dynamicHeaders].unpackedDir + "\\" + mapHeader.ToString("D4"), mapHeader, 0);
            } else {
                h = MapHeader.LoadFromARM9(mapHeader);
            }
            eventMatrixUpDown.Value = h.matrixID;

            eventMatrix = new GameMatrix((int)eventMatrixUpDown.Value);
            eventMatrixXUpDown.Maximum = eventMatrix.width - 1;
            eventMatrixYUpDown.Maximum = eventMatrix.height - 1;
            eventAreaDataUpDown.Value = h.areaDataID;
            eventEditorFullMapReload((int)eventMatrixXUpDown.Value, (int)eventMatrixYUpDown.Value);

            parent.disableHandlers = false;

            CenterEventViewOnEntities();
        }

        private void showEventsCheckBoxes_CheckedChanged(object sender, EventArgs e) {
            if (parent.disableHandlers) {
                return;
            }

            DisplayActiveEvents();
            Properties.Settings.Default.renderSpawnables = showSpawnablesCheckBox.Checked;
            Properties.Settings.Default.renderOverworlds = showOwsCheckBox.Checked;
            Properties.Settings.Default.renderWarps = showWarpsCheckBox.Checked;
            Properties.Settings.Default.renderTriggers = showTriggersCheckBox.Checked;
        }
        #region Spawnables Tab

        private void addSpawnableButton_Click(object sender, EventArgs e) {
            int spCount = currentEvFile.spawnables.Count;

            currentEvFile.spawnables.Add(new Spawnable((int)eventMatrixXUpDown.Value, (int)eventMatrixYUpDown.Value));

            spawnablesListBox.Items.Add("");
            spawnablesListBox.SelectedIndex = spCount;
            updateSelectedSpawnableName();
        }

        private void duplicateSpawnableButton_Click(object sender, EventArgs e) {
            if (spawnablesListBox.SelectedIndex < 0) {
                return;
            }

            currentEvFile.spawnables.Add(new Spawnable((Spawnable)selectedEvent));

            spawnablesListBox.Items.Add("");
            spawnablesListBox.SelectedIndex = spawnablesListBox.Items.Count - 1;
            updateSelectedSpawnableName();
        }

        private void removeSpawnableButton_Click(object sender, EventArgs e) {
            if (spawnablesListBox.SelectedIndex < 0) {
                return;
            }

            parent.disableHandlers = true;

            /* Remove trigger object from list and the corresponding entry in the ListBox */
            int spawnableNumber = spawnablesListBox.SelectedIndex;
            currentEvFile.spawnables.RemoveAt(spawnableNumber);
            spawnablesListBox.Items.RemoveAt(spawnableNumber);

            FillSpawnablesBox(); // Update ListBox

            parent.disableHandlers = false;

            if (spawnableNumber > 0) {
                spawnablesListBox.SelectedIndex = spawnableNumber - 1;
            } else {
                DisplayActiveEvents();
            }
        }

        private void spawnableDirComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            int selectedSpawnable = spawnablesListBox.SelectedIndex;
            if (parent.disableHandlers || selectedSpawnable < 0) {
                return;
            }

            currentEvFile.spawnables[selectedSpawnable].dir = (ushort)spawnableDirComboBox.SelectedIndex;
            updateSelectedSpawnableName();
        }

        private void spawnableMapXUpDown_ValueChanged(object sender, EventArgs e) {
            int selectedSpawnable = spawnablesListBox.SelectedIndex;
            if (parent.disableHandlers || selectedSpawnable < 0) {
                return;
            }

            currentEvFile.spawnables[selectedSpawnable].xMapPosition = (short)spawnableMapXUpDown.Value;
            DisplayActiveEvents();
        }

        private void spawnableMapYUpDown_ValueChanged(object sender, EventArgs e) {
            int selectedSpawnable = spawnablesListBox.SelectedIndex;
            if (parent.disableHandlers || selectedSpawnable < 0) {
                return;
            }

            currentEvFile.spawnables[selectedSpawnable].yMapPosition = (short)spawnableMapYUpDown.Value;
            DisplayActiveEvents();
        }

        private void spawnableMatrixXUpDown_ValueChanged(object sender, EventArgs e) {
            int selectedSpawnable = spawnablesListBox.SelectedIndex;
            if (parent.disableHandlers || selectedSpawnable < 0) {
                return;
            }

            currentEvFile.spawnables[selectedSpawnable].xMatrixPosition = (ushort)spawnableXMatrixUpDown.Value;
            DisplayActiveEvents();
        }

        private void spawnableMatrixYUpDown_ValueChanged(object sender, EventArgs e) {
            int selectedSpawnable = spawnablesListBox.SelectedIndex;
            if (parent.disableHandlers || selectedSpawnable < 0) {
                return;
            }

            currentEvFile.spawnables[selectedSpawnable].yMatrixPosition = (ushort)spawnableYMatrixUpDown.Value;
            DisplayActiveEvents();
        }

        private void spawnableScriptUpDown_ValueChanged(object sender, EventArgs e) {
            int selectedSpawnable = spawnablesListBox.SelectedIndex;
            if (parent.disableHandlers || selectedSpawnable < 0) {
                return;
            }

            currentEvFile.spawnables[selectedSpawnable].scriptNumber = (ushort)spawnableScriptUpDown.Value;
            updateSelectedSpawnableName();
        }

        private void spawnablesListBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (parent.disableHandlers || spawnablesListBox.SelectedIndex < 0)
                return;
            parent.disableHandlers = true;

            /* Set Event */
            selectedEvent = currentEvFile.spawnables[spawnablesListBox.SelectedIndex];

            /* Update Controls */
            spawnableDirComboBox.SelectedIndex = currentEvFile.spawnables[spawnablesListBox.SelectedIndex].dir;
            spawnableTypeComboBox.SelectedIndex = currentEvFile.spawnables[spawnablesListBox.SelectedIndex].type;

            spawnableScriptUpDown.Value = currentEvFile.spawnables[spawnablesListBox.SelectedIndex].scriptNumber;
            spawnableMapXUpDown.Value = currentEvFile.spawnables[spawnablesListBox.SelectedIndex].xMapPosition;
            spawnableMapYUpDown.Value = currentEvFile.spawnables[spawnablesListBox.SelectedIndex].yMapPosition;
            spawnableUpDown.Value = currentEvFile.spawnables[spawnablesListBox.SelectedIndex].zPosition;
            spawnableXMatrixUpDown.Value = currentEvFile.spawnables[spawnablesListBox.SelectedIndex].xMatrixPosition;
            spawnableYMatrixUpDown.Value = currentEvFile.spawnables[spawnablesListBox.SelectedIndex].yMatrixPosition;

            DisplayActiveEvents();
            parent.disableHandlers = false;
        }

        private void spawnableTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            int selectedSpawnable = spawnablesListBox.SelectedIndex;
            if (parent.disableHandlers || selectedSpawnable < 0) {
                return;
            }

            spawnableDirComboBox.Enabled = spawnableTypeComboBox.SelectedIndex != Spawnable.TYPE_HIDDENITEM;

            currentEvFile.spawnables[selectedSpawnable].type = (ushort)spawnableTypeComboBox.SelectedIndex;
            updateSelectedSpawnableName();
        }

        private void spawnableZUpDown_ValueChanged(object sender, EventArgs e) {
            int selectedSpawnable = spawnablesListBox.SelectedIndex;
            if (parent.disableHandlers || selectedSpawnable < 0) {
                return;
            }

            currentEvFile.spawnables[selectedSpawnable].zPosition = (short)spawnableUpDown.Value;
            DisplayActiveEvents();
        }

        #endregion Spawnables Tab

        #region Overworlds Tab

        private void addOverworldButton_Click(object sender, EventArgs e) {
            int owCount = currentEvFile.overworlds.Count;
            // Find the smallest unused ID
            int newID = 0;
            while (currentEvFile.overworlds.Any(ow => ow.owID == newID)) {
                newID++;
            }
            currentEvFile.overworlds.Add(new Overworld(newID, (int)eventMatrixXUpDown.Value, (int)eventMatrixYUpDown.Value));

            overworldsListBox.Items.Add("");
            overworldsListBox.SelectedIndex = owCount;
            updateSelectedOverworldName();
        }

        private void duplicateOverworldsButton_Click(object sender, EventArgs e) {
            if (overworldsListBox.SelectedIndex < 0) {
                return;
            }
            currentEvFile.overworlds.Add(new Overworld(selectedEvent as Overworld));

            overworldsListBox.Items.Add("");
            overworldsListBox.SelectedIndex = currentEvFile.overworlds.Count - 1;
            updateSelectedOverworldName();
        }

        private void overworldsListBox_SelectedIndexChanged(object sender, EventArgs e) {
            int index = overworldsListBox.SelectedIndex;

            if (parent.disableHandlers || index < 0) {
                return;
            }
            parent.disableHandlers = true;

            selectedEvent = currentEvFile.overworlds[index];
            Overworld selectedOw = (Overworld)selectedEvent;
            try {
                /* Sprite index and image controls */
                owSpriteComboBox.SelectedIndex = Array.IndexOf(RomInfo.overworldTableKeys, selectedOw.overlayTableEntry);
                owSpritePictureBox.BackgroundImage = GetOverworldImage(selectedOw.overlayTableEntry, selectedOw.orientation);
            } catch (ArgumentOutOfRangeException) {
                String errorMsg = "This Overworld's sprite ID couldn't be read correctly.";
                MessageBox.Show(errorMsg, "Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try {
                /* Special settings controls */
                if (selectedOw.type == (ushort)Overworld.OwType.TRAINER) {
                    isTrainerRadioButton.Checked = true;
                    if (selectedOw.scriptNumber >= 4999) {
                        owTrainerComboBox.SelectedIndex = Math.Max(selectedOw.scriptNumber - 4999, 0); // Partner of double battle trainer
                        owPartnerTrainerCheckBox.Checked = true;
                    } else {
                        owTrainerComboBox.SelectedIndex = Math.Max(selectedOw.scriptNumber - 2999, 0); // Normal trainer
                        owPartnerTrainerCheckBox.Checked = false;
                    }
                } else if (selectedOw.type == (ushort)Overworld.OwType.ITEM || selectedOw.scriptNumber >= 7000 && selectedOw.scriptNumber <= 8000) {
                    isItemRadioButton.Checked = true;
                    owItemComboBox.SelectedIndex = Math.Max(selectedOw.scriptNumber - 7000, 0);
                } else {
                    normalRadioButton.Checked = true;
                }

                /* Set coordinates controls */
                owXMapUpDown.Value = selectedOw.xMapPosition;
                owYMapUpDown.Value = selectedOw.yMapPosition;
                owXMatrixUpDown.Value = selectedOw.xMatrixPosition;
                owYMatrixUpDown.Value = selectedOw.yMatrixPosition;

                int zFld = (selectedOw.zPosition >> 4) /*convert from overworld units to fx*/;
                owZPositionUpDown.Value = zFld / 0x1000; //then convert from fixed point to decimal

                /*ID, Flag and Script number controls */
                owIDNumericUpDown.Value = selectedOw.owID;
                owFlagNumericUpDown.Value = selectedOw.flag;
                owScriptNumericUpDown.Value = selectedOw.scriptNumber;

                /* Movement settings */
                owMovementComboBox.SelectedIndex = selectedOw.movement;
                owOrientationComboBox.SelectedIndex = selectedOw.orientation;
                owSightRangeUpDown.Value = selectedOw.sightRange;
                owXRangeUpDown.Value = selectedOw.xRange;
                owYRangeUpDown.Value = selectedOw.yRange;

                try {
                    uint spriteID = RomInfo.OverworldTable[currentEvFile.overworlds[overworldsListBox.SelectedIndex].overlayTableEntry].spriteID;
                    if (spriteID == 0x3D3D) {
                        spriteIDlabel.Text = "3D Overworld";
                    } else {
                        spriteIDlabel.Text = "Sprite ID: " + spriteID;
                    }
                } catch { }
                DisplayActiveEvents();
            } catch (ArgumentOutOfRangeException) {
                String errorMsg = "There was a problem loading the overworld events of this Event file.";
                MessageBox.Show(errorMsg, "Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            parent.disableHandlers = false;
        }

        private void owFlagNumericUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = overworldsListBox.SelectedIndex;

            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.overworlds[selection].flag = (ushort)owFlagNumericUpDown.Value;
        }

        private void owIDNumericUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = overworldsListBox.SelectedIndex;

            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.overworlds[selection].owID = (ushort)owIDNumericUpDown.Value;
            updateSelectedOverworldName();
        }

        private void owItemComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (parent.disableHandlers || overworldsListBox.SelectedIndex < 0) {
                return;
            }

            owScriptNumericUpDown.Value = currentEvFile.overworlds[overworldsListBox.SelectedIndex].scriptNumber = (ushort)(7000 + owItemComboBox.SelectedIndex);
        }

        private void owMovementComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            int selection = overworldsListBox.SelectedIndex;

            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.overworlds[selection].movement = (ushort)owMovementComboBox.SelectedIndex;
        }

        private void owOrientationComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            ushort orientation = (ushort)owOrientationComboBox.SelectedIndex;
            int selection = overworldsListBox.SelectedIndex;
            if (owSpriteComboBox.SelectedIndex < 0 || orientation < 0) {
                return;
            }

            if (selection >= 0) {
                owSpritePictureBox.BackgroundImage = GetOverworldImage(currentEvFile.overworlds[selection].overlayTableEntry, orientation);

                if (!parent.disableHandlers) {
                    currentEvFile.overworlds[selection].orientation = orientation;
                    DisplayActiveEvents();
                }
            } else {
                owSpritePictureBox.BackgroundImage = GetOverworldImage((ushort)owSpriteComboBox.SelectedIndex, orientation);
            }

            owSpritePictureBox.Invalidate();
        }

        private void owPartnerTrainerCheckBox_CheckedChanged(object sender, EventArgs e) {
            int selection = overworldsListBox.SelectedIndex;

            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.overworlds[selection].scriptNumber = (ushort)(currentEvFile.overworlds[selection].scriptNumber + (owPartnerTrainerCheckBox.Checked ? 2000 : -2000));
        }

        private void owScriptNumericUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = overworldsListBox.SelectedIndex;

            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.overworlds[selection].scriptNumber = (ushort)owScriptNumericUpDown.Value;
            updateSelectedOverworldName();
        }

        private void owSightRangeUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = overworldsListBox.SelectedIndex;

            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.overworlds[selection].sightRange = (ushort)owSightRangeUpDown.Value;
        }

        private void owSpriteComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            int selection = overworldsListBox.SelectedIndex;
            if (owSpriteComboBox.SelectedIndex < 0) {
                return;
            }
            ushort overlayTableEntryID = (ushort)RomInfo.OverworldTable.Keys.ElementAt(owSpriteComboBox.SelectedIndex);
            uint spriteID = RomInfo.OverworldTable[overlayTableEntryID].spriteID;

            if (spriteID == 0x3D3D) {
                spriteIDlabel.Text = "3D Overworld";
            } else {
                spriteIDlabel.Text = "Sprite ID: " + spriteID;
            }

            if (selection >= 0) {
                owSpritePictureBox.BackgroundImage = GetOverworldImage(overlayTableEntryID, currentEvFile.overworlds[selection].orientation);

                if (!parent.disableHandlers) {
                    currentEvFile.overworlds[selection].overlayTableEntry = overlayTableEntryID;
                    DisplayActiveEvents();
                }
            } else {
                owSpritePictureBox.BackgroundImage = GetOverworldImage(overlayTableEntryID, (ushort)owOrientationComboBox.SelectedIndex);
            }
            owSpritePictureBox.Invalidate();
            updateSelectedOverworldName();
        }

        private void owTrainerComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            int selection = overworldsListBox.SelectedIndex;

            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.overworlds[selection].scriptNumber = (ushort)(owTrainerComboBox.SelectedIndex + (owPartnerTrainerCheckBox.Checked ? 4999 : 2999));
        }

        private void OWTypeChanged(object sender, EventArgs e) {
            if (overworldsListBox.SelectedIndex < 0) {
                return;
            }

            if (normalRadioButton.Checked == true) {
                owScriptNumericUpDown.Enabled = true;
                owSpecialGroupBox.Enabled = false;

                if (parent.disableHandlers) {
                    return;
                }
                currentEvFile.overworlds[overworldsListBox.SelectedIndex].type = 0x0;
                currentEvFile.overworlds[overworldsListBox.SelectedIndex].scriptNumber = (ushort)(owScriptNumericUpDown.Value = 0);
            } else if (isItemRadioButton.Checked == true) {
                owScriptNumericUpDown.Enabled = false;

                owSpecialGroupBox.Enabled = true;
                owTrainerComboBox.Enabled = false;
                owTrainerLabel.Enabled = false;
                owSightRangeUpDown.Enabled = false;
                owSightRangeLabel.Enabled = false;
                owPartnerTrainerCheckBox.Enabled = false;

                if (isItemRadioButton.Enabled) {
                    owItemComboBox.Enabled = true;
                    itemsSelectorHelpBtn.Enabled = true;
                    owItemLabel.Enabled = true;

                    if (parent.disableHandlers) {
                        return;
                    }
                    currentEvFile.overworlds[overworldsListBox.SelectedIndex].type = 0x3;
                    currentEvFile.overworlds[overworldsListBox.SelectedIndex].scriptNumber = (ushort)(owScriptNumericUpDown.Value = 7000 + owItemComboBox.SelectedIndex);
                }
            } else { //trainer
                owScriptNumericUpDown.Enabled = false;

                owSpecialGroupBox.Enabled = true;
                owTrainerComboBox.Enabled = true;
                owTrainerLabel.Enabled = true;
                owItemLabel.Enabled = false;
                owSightRangeUpDown.Enabled = true;
                owSightRangeLabel.Enabled = true;
                owPartnerTrainerCheckBox.Enabled = true;

                owItemComboBox.Enabled = false;
                itemsSelectorHelpBtn.Enabled = false;

                if (parent.disableHandlers) {
                    return;
                }
                currentEvFile.overworlds[overworldsListBox.SelectedIndex].type = 0x1;
                if (owTrainerComboBox.SelectedIndex >= 0) {
                    owTrainerComboBox_SelectedIndexChanged(null, null);
                }
            }
        }

        private void owXMapUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = overworldsListBox.SelectedIndex;

            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.overworlds[overworldsListBox.SelectedIndex].xMapPosition = (short)owXMapUpDown.Value;
            DisplayActiveEvents();
        }

        private void owXMatrixUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = overworldsListBox.SelectedIndex;

            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.overworlds[selection].xMatrixPosition = (ushort)owXMatrixUpDown.Value;
            eventMatrixPictureBox.Image = new Bitmap(eventMatrixPictureBox.Width, eventMatrixPictureBox.Height);
            MarkActiveCell((int)eventMatrixXUpDown.Value, (int)eventMatrixYUpDown.Value);
            DisplayActiveEvents();
        }

        private void owXRangeUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = overworldsListBox.SelectedIndex;

            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.overworlds[overworldsListBox.SelectedIndex].xRange = (ushort)owXRangeUpDown.Value;
        }

        private void owYMapUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = overworldsListBox.SelectedIndex;

            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.overworlds[overworldsListBox.SelectedIndex].yMapPosition = (short)owYMapUpDown.Value;
            DisplayActiveEvents();
        }

        private void owYMatrixUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = overworldsListBox.SelectedIndex;
            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.overworlds[selection].yMatrixPosition = (ushort)owYMatrixUpDown.Value;
            eventMatrixPictureBox.Image = new Bitmap(eventMatrixPictureBox.Width, eventMatrixPictureBox.Height);
            MarkActiveCell((int)eventMatrixXUpDown.Value, (int)eventMatrixYUpDown.Value);
            DisplayActiveEvents();
        }

        private void owYRangeUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = overworldsListBox.SelectedIndex;

            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.overworlds[overworldsListBox.SelectedIndex].yRange = (ushort)owYRangeUpDown.Value;
        }

        private void owZPositionUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = overworldsListBox.SelectedIndex;

            if (parent.disableHandlers || selection < 0) {
                return;
            }

            int zFld = (short)owZPositionUpDown.Value << 4; //decimal to overworld units
            currentEvFile.overworlds[selection].zPosition = zFld * 0x1000; //overworld units to fixed point
        }

        private void removeOverworldButton_Click(object sender, EventArgs e) {
            if (overworldsListBox.SelectedIndex < 0) {
                return;
            }

            parent.disableHandlers = true;

            /* Remove overworld object from list and the corresponding entry in the ListBox */
            int owNumber = overworldsListBox.SelectedIndex;
            currentEvFile.overworlds.RemoveAt(owNumber);
            overworldsListBox.Items.RemoveAt(owNumber);

            FillOverworldsBox(); // Update ListBox
            parent.disableHandlers = false;

            if (owNumber > 0) {
                overworldsListBox.SelectedIndex = owNumber - 1;
            } else {
                DisplayActiveEvents();
            }
        }

        private void sortOWsByIDAscButton_Click(object sender, EventArgs e) {
            currentEvFile.overworlds.Sort((x, y) => x.owID.CompareTo(y.owID));
            overworldsListBox.BeginUpdate();
            FillOverworldsBox();
            overworldsListBox.EndUpdate();
        }

        private void sortOWsByIDDescButton_Click(object sender, EventArgs e) {
            currentEvFile.overworlds.Sort((x, y) => y.owID.CompareTo(x.owID));
            overworldsListBox.BeginUpdate();
            FillOverworldsBox();
            overworldsListBox.EndUpdate();
        }

        #endregion Overworlds Tab

        #region Warps Tab

        private void addWarpButton_Click(object sender, EventArgs e) {
            Warp n = new Warp((int)eventMatrixXUpDown.Value, (int)eventMatrixYUpDown.Value);
            currentEvFile.warps.Add(n);

            int index = currentEvFile.warps.Count - 1;

            warpsListBox.Items.Add("");
            warpsListBox.SelectedIndex = index;
            updateSelectedWarpName();

            eventEditorWarpHeaderListBox.SelectedIndex = n.header;
        }

        private void duplicateWarpsButton_Click(object sender, EventArgs e) {
            if (warpsListBox.SelectedIndex < 0) {
                return;
            }

            Warp n = new Warp(selectedEvent as Warp);
            currentEvFile.warps.Add(n);

            int index = currentEvFile.warps.Count - 1;

            warpsListBox.Items.Add("");
            warpsListBox.SelectedIndex = index;
            updateSelectedWarpName();

            eventEditorWarpHeaderListBox.SelectedIndex = n.header;
        }

        private void eventEditorWarpHeaderListBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (eventEditorWarpHeaderListBox.SelectedIndex < 0) {
                eventEditorHeaderLocationNameLabel.Text = "";
                return;
            }

            ushort destHeaderID = (ushort)eventEditorWarpHeaderListBox.SelectedIndex;

            MapHeader destHeader;
            if (ROMToolboxDialog.flag_DynamicHeadersPatchApplied || ROMToolboxDialog.CheckFilesDynamicHeadersPatchApplied()) {
                destHeader = MapHeader.LoadFromFile(RomInfo.gameDirs[DirNames.dynamicHeaders].unpackedDir + "\\" + destHeaderID.ToString("D4"), destHeaderID, 0);
            } else {
                destHeader = MapHeader.LoadFromARM9(destHeaderID);
            }

            int locNum;
            switch (RomInfo.gameFamily) {
                case gFamEnum.DP: {
                        HeaderDP h = (HeaderDP)destHeader;

                        locNum = h.locationName;
                        break;
                    }
                case gFamEnum.Plat: {
                        HeaderPt h = (HeaderPt)destHeader;

                        locNum = h.locationName;
                        break;
                    }
                default: {
                        HeaderHGSS h = (HeaderHGSS)destHeader;

                        locNum = h.locationName;
                        break;
                    }
            }

            eventEditorHeaderLocationNameLabel.Text = (string)parent.GetLocationNameComboBox().Items[locNum];

            int warpSel = warpsListBox.SelectedIndex;
            if (parent.disableHandlers || warpSel < 0) {
                return;
            }
            currentEvFile.warps[warpSel].header = destHeaderID;
            updateSelectedWarpName();
        }

        private void goToWarpDestination_Click(object sender, EventArgs e) {
            if (warpsListBox.SelectedIndex < 0) {
                return;
            }

            int destAnchor = (int)warpAnchorUpDown.Value;
            ushort destHeaderID = (ushort)eventEditorWarpHeaderListBox.SelectedIndex;

            MapHeader destHeader;
            if (ROMToolboxDialog.flag_DynamicHeadersPatchApplied || ROMToolboxDialog.CheckFilesDynamicHeadersPatchApplied()) {
                destHeader = MapHeader.LoadFromFile(RomInfo.gameDirs[DirNames.dynamicHeaders].unpackedDir + "\\" + destHeaderID.ToString("D4"), destHeaderID, 0);
            } else {
                destHeader = MapHeader.LoadFromARM9(destHeaderID);
            }

            if (new EventFile(destHeader.eventFileID).warps.Count < destAnchor + 1) {
                DialogResult d = MessageBox.Show("The selected warp's destination anchor doesn't exist.\n" +
                    "Do you want to open the destination map anyway?", "Warp is not connected", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (d == DialogResult.No)
                    return;
                else {
                    eventMatrixUpDown.Value = destHeader.matrixID;
                    eventAreaDataUpDown.Value = destHeader.areaDataID;
                    selectEventComboBox.SelectedIndex = destHeader.eventFileID;
                    CenterEventViewOnEntities();
                    return;
                }
            }

            eventMatrixUpDown.Value = destHeader.matrixID;
            eventAreaDataUpDown.Value = destHeader.areaDataID;
            selectEventComboBox.SelectedIndex = destHeader.eventFileID;

            warpsListBox.SelectedIndex = destAnchor;
            centerEventViewOnSelectedEvent_Click(sender, e);
        }

        private void removeWarpButton_Click(object sender, EventArgs e) {
            if (warpsListBox.SelectedIndex < 0) {
                return;
            }

            parent.disableHandlers = true;

            /* Remove warp object from list and the corresponding entry in the ListBox */
            int warpNumber = warpsListBox.SelectedIndex;
            currentEvFile.warps.RemoveAt(warpNumber);
            warpsListBox.Items.RemoveAt(warpNumber);

            FillWarpsBox(); // Update ListBox

            parent.disableHandlers = false;

            if (warpNumber > 0) {
                warpsListBox.SelectedIndex = warpNumber - 1;
            } else {
                DisplayActiveEvents();
            }
        }

        private void warpAnchorUpDown_ValueChanged(object sender, EventArgs e) {
            if (parent.disableHandlers || warpsListBox.SelectedIndex < 0) {
                return;
            }

            currentEvFile.warps[warpsListBox.SelectedIndex].anchor = (ushort)warpAnchorUpDown.Value;
            updateSelectedWarpName();
        }

        private void warpMatrixXUpDown_ValueChanged(object sender, EventArgs e) {
            int index = warpsListBox.SelectedIndex;
            if (parent.disableHandlers || index < 0) {
                return;
            }

            currentEvFile.warps[index].xMatrixPosition = (ushort)warpXMatrixUpDown.Value;
            DisplayActiveEvents();
        }

        private void warpMatrixYUpDown_ValueChanged(object sender, EventArgs e) {
            int index = warpsListBox.SelectedIndex;
            if (parent.disableHandlers || index < 0) {
                return;
            }

            currentEvFile.warps[index].yMatrixPosition = (ushort)warpYMatrixUpDown.Value;
            DisplayActiveEvents();
        }

        private void warpsListBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (parent.disableHandlers || warpsListBox.SelectedIndex < 0) {
                return;
            }

            selectedEvent = currentEvFile.warps[warpsListBox.SelectedIndex];
            eventEditorWarpHeaderListBox.SelectedIndex = currentEvFile.warps[warpsListBox.SelectedIndex].header;

            parent.disableHandlers = true;

            warpAnchorUpDown.Value = currentEvFile.warps[warpsListBox.SelectedIndex].anchor;
            warpXMapUpDown.Value = currentEvFile.warps[warpsListBox.SelectedIndex].xMapPosition;
            warpYMapUpDown.Value = currentEvFile.warps[warpsListBox.SelectedIndex].yMapPosition;
            warpZUpDown.Value = currentEvFile.warps[warpsListBox.SelectedIndex].zPosition;
            warpXMatrixUpDown.Value = currentEvFile.warps[warpsListBox.SelectedIndex].xMatrixPosition;
            warpYMatrixUpDown.Value = currentEvFile.warps[warpsListBox.SelectedIndex].yMatrixPosition;

            DisplayActiveEvents(); // Redraw events to show selection box

            #region Re-enable events

            parent.disableHandlers = false;

            #endregion Re-enable events
        }

        private void warpXMapUpDown_ValueChanged(object sender, EventArgs e) {
            int index = warpsListBox.SelectedIndex;
            if (parent.disableHandlers || index < 0) {
                return;
            }

            currentEvFile.warps[index].xMapPosition = (short)warpXMapUpDown.Value;
            DisplayActiveEvents();
        }

        private void warpYMapUpDown_ValueChanged(object sender, EventArgs e) {
            int index = warpsListBox.SelectedIndex;
            if (parent.disableHandlers || index < 0) {
                return;
            }

            currentEvFile.warps[index].yMapPosition = (short)warpYMapUpDown.Value;
            DisplayActiveEvents();
        }

        private void warpZUpDown_ValueChanged(object sender, EventArgs e) {
            int index = warpsListBox.SelectedIndex;
            if (parent.disableHandlers || index < 0) {
                return;
            }

            currentEvFile.warps[index].zPosition = (short)warpZUpDown.Value;
            DisplayActiveEvents();
        }

        #endregion Warps Tab

        #region Triggers Tab

        private void addTriggerButton_Click(object sender, EventArgs e) {
            currentEvFile.triggers.Add(new Trigger((int)eventMatrixXUpDown.Value, (int)eventMatrixYUpDown.Value));

            triggersListBox.Items.Add("");
            triggersListBox.SelectedIndex = currentEvFile.triggers.Count - 1;
            updateSelectedTriggerName();
        }

        private void duplicateTriggersButton_Click(object sender, EventArgs e) {
            if (triggersListBox.SelectedIndex < 0) {
                return;
            }

            currentEvFile.triggers.Add(new Trigger(selectedEvent as Trigger));

            triggersListBox.Items.Add("");
            triggersListBox.SelectedIndex = currentEvFile.triggers.Count - 1;
            updateSelectedTriggerName();
        }

        private void expectedVarValueTriggerUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = triggersListBox.SelectedIndex;
            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.triggers[selection].expectedVarValue = (ushort)expectedVarValueTriggerUpDown.Value;
            updateSelectedTriggerName();
        }

        private void removeTriggerButton_Click(object sender, EventArgs e) {
            int selection = triggersListBox.SelectedIndex;

            if (selection < 0) {
                return;
            }

            parent.disableHandlers = true;

            /* Remove trigger object from list and the corresponding entry in the ListBox */
            currentEvFile.triggers.RemoveAt(selection);
            triggersListBox.Items.RemoveAt(selection);

            FillTriggersBox(); // Update ListBox

            parent.disableHandlers = false;

            if (selection > 0) {
                triggersListBox.SelectedIndex = selection - 1;
            } else {
                DisplayActiveEvents();
            }
        }

        private void triggerLengthUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = triggersListBox.SelectedIndex;
            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.triggers[selection].heightY = (ushort)triggerLengthUpDown.Value;
            DisplayActiveEvents();
        }

        private void triggerScriptUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = triggersListBox.SelectedIndex;
            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.triggers[selection].scriptNumber = (ushort)triggerScriptUpDown.Value;
            updateSelectedTriggerName();
        }

        private void triggersListBox_SelectedIndexChanged(object sender, EventArgs e) {
            int selectedIndex = triggersListBox.SelectedIndex;

            if (parent.disableHandlers || selectedIndex < 0) {
                return;
            }
            parent.disableHandlers = true;

            Trigger t = (selectedEvent = currentEvFile.triggers[selectedIndex]) as Trigger;

            triggerScriptUpDown.Value = t.scriptNumber;
            triggerVariableWatchedUpDown.Value = t.variableWatched;
            expectedVarValueTriggerUpDown.Value = t.expectedVarValue;

            triggerWidthUpDown.Value = t.widthX;
            triggerLengthUpDown.Value = t.heightY;

            triggerXMapUpDown.Value = t.xMapPosition;
            triggerYMapUpDown.Value = t.yMapPosition;
            triggerZUpDown.Value = t.zPosition;
            triggerXMatrixUpDown.Value = t.xMatrixPosition;
            triggerYMatrixUpDown.Value = t.yMatrixPosition;

            DisplayActiveEvents();

            parent.disableHandlers = false;
        }

        private void triggerVariableWatchedUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = triggersListBox.SelectedIndex;
            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.triggers[selection].variableWatched = (ushort)triggerVariableWatchedUpDown.Value;
            updateSelectedTriggerName();
        }

        private void triggerWidthUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = triggersListBox.SelectedIndex;
            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.triggers[selection].widthX = (ushort)triggerWidthUpDown.Value;
            DisplayActiveEvents();
        }

        private void triggerXMapUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = triggersListBox.SelectedIndex;
            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.triggers[selection].xMapPosition = (short)triggerXMapUpDown.Value;
            DisplayActiveEvents();
        }

        private void triggerXMatrixUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = triggersListBox.SelectedIndex;
            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.triggers[selection].xMatrixPosition = (ushort)triggerXMatrixUpDown.Value;
            DisplayActiveEvents();
        }

        private void triggerYMapUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = triggersListBox.SelectedIndex;
            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.triggers[selection].yMapPosition = (short)triggerYMapUpDown.Value;
            DisplayActiveEvents();
        }

        private void triggerYMatrixUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = triggersListBox.SelectedIndex;
            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.triggers[selection].yMatrixPosition = (ushort)triggerYMatrixUpDown.Value;
            DisplayActiveEvents();
        }

        private void triggerZUpDown_ValueChanged(object sender, EventArgs e) {
            int selection = triggersListBox.SelectedIndex;
            if (parent.disableHandlers || selection < 0) {
                return;
            }

            currentEvFile.triggers[selection].zPosition = (ushort)triggerZUpDown.Value;
            DisplayActiveEvents();
        }

        #endregion Triggers Tab

        #endregion EventHandlers
    }
}