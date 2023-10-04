﻿using DSPRE.ROMFiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static DSPRE.RomInfo;

namespace DSPRE {

    public partial class MatrixEditor : UserControl {
        public GameMatrix currentMatrix;
        private MainProgram parent;

        public MatrixEditor() {
            InitializeComponent();
            //parent = (Parent as MainProgram);
        }

        #region Subroutines

        private void ClearMatrixTables() {
            headersGridView.Rows.Clear();
            headersGridView.Columns.Clear();
            heightsGridView.Rows.Clear();
            heightsGridView.Columns.Clear();
            mapFilesGridView.Rows.Clear();
            mapFilesGridView.Columns.Clear();
            matrixTabControl.TabPages.Remove(headersTabPage);
            matrixTabControl.TabPages.Remove(heightsTabPage);
        }

        private (Color background, Color foreground) FormatMapCell(uint cellValue) {
            foreach (KeyValuePair<List<uint>, (Color background, Color foreground)> entry in parent.GetRomInfo().MapCellsColorDictionary) {
                if (entry.Key.Contains(cellValue))
                    return entry.Value;
            }
            return (Color.White, Color.Black);
        }

        public string GetCurrentMatrixName() {
            return selectMatrixComboBox.SelectedIndex.ToString("D4");
        }

        public void LoadFromHeader(int matrixToLoad) {
            selectMatrixComboBox.SelectedIndex = matrixToLoad;

            if (currentMatrix.hasHeadersSection) {
                matrixTabControl.SelectedTab = headersTabPage;

                //Autoselect cell containing current header, if such cell exists [and if current matrix has headers sections]
                for (int i = 0; i < headersGridView.RowCount; i++) {
                    for (int j = 0; j < headersGridView.ColumnCount; j++) {
                        if (parent.currentHeader.ID.ToString() == headersGridView.Rows[i].Cells[j].Value.ToString()) {
                            headersGridView.CurrentCell = headersGridView.Rows[i].Cells[j];
                            return;
                        }
                    }
                }
            }
        }

        private void GenerateMatrixTables() {
            /* Generate table columns */
            if (currentMatrix is null) {
                return;
            }

            for (int i = 0; i < currentMatrix.width; i++) {
                headersGridView.Columns.Add("Column" + i, i.ToString("D"));
                headersGridView.Columns[i].Width = 32; // Set column size
                headersGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                headersGridView.Columns[i].Frozen = false;

                heightsGridView.Columns.Add("Column" + i, i.ToString("D"));
                heightsGridView.Columns[i].Width = 21; // Set column size
                heightsGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                heightsGridView.Columns[i].Frozen = false;

                mapFilesGridView.Columns.Add("Column" + i, i.ToString("D"));
                mapFilesGridView.Columns[i].Width = 32; // Set column size
                mapFilesGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                mapFilesGridView.Columns[i].Frozen = false;
            }

            /* Generate table rows */
            for (int i = 0; i < currentMatrix.height; i++) {
                mapFilesGridView.Rows.Add();
                mapFilesGridView.Rows[i].HeaderCell.Value = i.ToString();

                headersGridView.Rows.Add();
                headersGridView.Rows[i].HeaderCell.Value = i.ToString();

                heightsGridView.Rows.Add();
                heightsGridView.Rows[i].HeaderCell.Value = i.ToString();
            }

            /* Fill tables */
            for (int i = 0; i < currentMatrix.height; i++) {
                for (int j = 0; j < currentMatrix.width; j++) {
                    headersGridView.Rows[i].Cells[j].Value = currentMatrix.headers[i, j];
                    heightsGridView.Rows[i].Cells[j].Value = currentMatrix.altitudes[i, j];
                    mapFilesGridView.Rows[i].Cells[j].Value = currentMatrix.maps[i, j];
                }
            }

            if (currentMatrix.hasHeadersSection) {
                matrixTabControl.TabPages.Add(headersTabPage);
            }

            if (currentMatrix.hasHeightsSection) {
                matrixTabControl.TabPages.Add(heightsTabPage);
            }
        }

        public void SetupMatrixEditor(MainProgram main) {
            parent = main;
            DSUtils.TryUnpackNarcs(new List<DirNames> { DirNames.matrices });

            parent.disableHandlers = true;

            /* Add matrix entries to ComboBox */
            selectMatrixComboBox.Items.Clear();
            selectMatrixComboBox.Items.Add("Matrix 0 - Main");
            for (int i = 1; i < parent.GetRomInfo().GetMatrixCount(); i++) {
                selectMatrixComboBox.Items.Add(new GameMatrix(i));
            }

            if (!ReadColorTable(Properties.Settings.Default.lastColorTablePath, silent: true)) {
                parent.GetRomInfo().ResetMapCellsColorDictionary();
            }
            RomInfo.SetupSpawnSettings();

            parent.disableHandlers = false;
            selectMatrixComboBox.SelectedIndex = 0;
        }

        private bool ReadColorTable(string fileName, bool silent) {
            if (string.IsNullOrWhiteSpace(fileName)) {
                return false;
            }

            string[] fileTableContent = File.ReadAllLines(fileName);

            if (fileTableContent.Length > 0) {
                const string mapKeyword = "[Maplist]";
                const string colorKeyword = "[Color]";
                const string textColorKeyword = "[TextColor]";
                const string dashSeparator = "-";
                string problematicSegment = "incomplete line";

                Dictionary<List<uint>, (Color background, Color foreground)> colorsDict = new Dictionary<List<uint>, (Color background, Color foreground)>();
                List<string> linesWithErrors = new List<string>();

                for (int i = 0; i < fileTableContent.Length; i++) {
                    if (fileTableContent[i].Length > 0) {
                        string[] lineParts = fileTableContent[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        try {
                            int j = 0;
                            if (!lineParts[j].Equals(mapKeyword)) {
                                problematicSegment = nameof(mapKeyword);
                                throw new FormatException();
                            }
                            j++;

                            List<uint> mapList = new List<uint>();
                            while (!lineParts[j].Equals(dashSeparator)) {
                                if (lineParts[j].Equals("and")) {
                                    j++;
                                }
                                uint firstValue = uint.Parse(lineParts[j++]);
                                mapList.Add(firstValue);

                                if (lineParts[j].Equals("to")) {
                                    j++;
                                    uint finalValue = uint.Parse(lineParts[j++]);
                                    //Add all numbers ranging from maplist[0] to finalValue
                                    if (firstValue > finalValue)
                                        Swap(ref firstValue, ref finalValue);

                                    for (uint k = firstValue + 1; k <= finalValue; k++) {
                                        mapList.Add(k);
                                    }
                                }
                            }

                            if (!lineParts[j].Equals(dashSeparator)) {
                                problematicSegment = nameof(dashSeparator);
                                throw new FormatException();
                            }
                            j++;

                            if (!lineParts[j].Equals(colorKeyword)) {
                                problematicSegment = nameof(colorKeyword);
                                throw new FormatException();
                            }
                            j++;

                            int r = Int32.Parse(lineParts[j++]);
                            int g = Int32.Parse(lineParts[j++]);
                            int b = Int32.Parse(lineParts[j++]);

                            if (!lineParts[j].Equals(dashSeparator)) {
                                problematicSegment = nameof(dashSeparator);
                                throw new FormatException();
                            }
                            j++;

                            if (!lineParts[j].Equals(textColorKeyword)) {
                                problematicSegment = nameof(textColorKeyword);
                                throw new FormatException();
                            }
                            j++;

                            colorsDict.Add(mapList, (Color.FromArgb(r, g, b), Color.FromName(lineParts[j++])));
                        } catch {
                            if (!silent) {
                                linesWithErrors.Add(i + 1 + " (err. " + problematicSegment + ")\n");
                            }
                            continue;
                        }
                    }
                }
                colorsDict.Add(new List<uint> { GameMatrix.EMPTY }, (Color.Black, Color.White));

                string errorMsg = "";
                MessageBoxIcon iconType = MessageBoxIcon.Information;

                if (!silent) {
                    if (linesWithErrors.Count > 0) {
                        errorMsg = "\nHowever, the following lines couldn't be parsed correctly:\n";

                        foreach (string s in linesWithErrors) {
                            errorMsg += "- Line " + s;
                        }

                        iconType = MessageBoxIcon.Warning;
                    }
                }
                parent.GetRomInfo().MapCellsColorDictionary = colorsDict;
                ClearMatrixTables();
                GenerateMatrixTables();

                Properties.Settings.Default.lastColorTablePath = fileName;

                if (!silent) {
                    MessageBox.Show("Color file has been read." + errorMsg, "Operation completed", MessageBoxButtons.OK, iconType);
                }
                return true;
            } else {
                if (!silent) {
                    MessageBox.Show("No readable content was found in this file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
        }

        public void Swap(ref uint a, ref uint b) {
            uint temp = a;
            a = b;
            b = temp;
        }

        #endregion Subroutines

        private void addHeaderSectionButton_Click(object sender, EventArgs e) {
            if (!currentMatrix.hasHeadersSection) {
                currentMatrix.hasHeadersSection = true;
                matrixTabControl.TabPages.Add(headersTabPage);
            }
        }

        private void addHeightsButton_Click(object sender, EventArgs e) {
            if (!currentMatrix.hasHeightsSection) {
                currentMatrix.hasHeightsSection = true;
                matrixTabControl.TabPages.Add(heightsTabPage);
            }
        }

        private void addMatrixButton_Click(object sender, EventArgs e) {
            GameMatrix blankMatrix = new GameMatrix();

            /* Add new matrix file to matrix folder */
            blankMatrix.SaveToFile(RomInfo.gameDirs[DirNames.matrices].unpackedDir + "\\" + parent.GetRomInfo().GetMatrixCount().ToString("D4"), false);

            /* Update ComboBox*/
            selectMatrixComboBox.Items.Add(selectMatrixComboBox.Items.Count.ToString() + blankMatrix);
            selectMatrixComboBox.SelectedIndex = selectMatrixComboBox.Items.Count - 1;

            if (parent.eventEditorIsReady) {
                (parent.GetEditor("Event") as EventEditor).Modify_eventMatrixUpDown_Max(true); // TODO: Replace with EventEditor.cs control
            }
        }

        private void exportMatrixButton_Click(object sender, EventArgs e) {
            currentMatrix.SaveToFileExplorePath("Matrix " + selectMatrixComboBox.SelectedIndex);
        }

        private void saveMatrixButton_Click(object sender, EventArgs e) {
            currentMatrix.SaveToFileDefaultDir(selectMatrixComboBox.SelectedIndex);
            GameMatrix saved = new GameMatrix(selectMatrixComboBox.SelectedIndex);
            selectMatrixComboBox.Items[selectMatrixComboBox.SelectedIndex] = saved.ToString();
            (parent.GetEditor("Event") as EventEditor).eventMatrix = saved; // TODO: Replace with EventEditor.cs control
        }

        private void headersGridView_SelectionChanged(object sender, EventArgs e) {
            DisplaySelection(headersGridView.SelectedCells);
        }

        private void heightsGridView_SelectionChanged(object sender, EventArgs e) {
            DisplaySelection(heightsGridView.SelectedCells);
        }

        private void mapFilesGridView_SelectionChanged(object sender, EventArgs e) {
            DisplaySelection(mapFilesGridView.SelectedCells);
        }

        private void DisplaySelection(DataGridViewSelectedCellCollection selectedCells) {
            if (selectedCells.Count > 0) {
                parent.statusLabelMessage("Selection:   " + selectedCells[0].ColumnIndex + ", " + selectedCells[0].RowIndex);
            }
        }

        private void headersGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (parent.GetHeaderListBox().Items.Count < parent.internalNames.Count) {
                HeaderSearch.ResetResults(parent.GetHeaderListBox(), parent.headerListBoxNames, prependNumbers: false);
            }

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) {
                int headerNumber = Convert.ToInt32(headersGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                parent.GetHeaderListBox().SelectedIndex = headerNumber;
                parent.ChangeTab("Header");
            }
        }

        private void headersGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            if (parent.disableHandlers) {
                return;
            }
            if (e.RowIndex > -1 && e.ColumnIndex > -1) {
                /* If input is junk, use 0000 as placeholder value */
                ushort cellValue;
                try {
                    if (!ushort.TryParse(headersGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out cellValue)) {
                        throw new NullReferenceException();
                    }
                } catch (NullReferenceException) {
                    cellValue = 0;
                }
                /* Change value in matrix object */
                currentMatrix.headers[e.RowIndex, e.ColumnIndex] = cellValue;
            }
        }

        private void headersGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (e.Value is null) {
                return;
            }

            parent.disableHandlers = true;

            /* Format table cells corresponding to border maps or void */
            if (!ushort.TryParse(mapFilesGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out ushort colorValue)) {
                colorValue = GameMatrix.EMPTY;
            }

            (Color back, Color fore) = FormatMapCell(colorValue);
            e.CellStyle.BackColor = back;
            e.CellStyle.ForeColor = fore;

            /* If invalid input is entered, show 00 */
            if (!ushort.TryParse(headersGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out _)) {
                e.Value = 0;
            }

            parent.disableHandlers = false;
        }

        private void heightsGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            if (parent.disableHandlers) {
                return;
            }
            if (e.RowIndex > -1 && e.ColumnIndex > -1) {
                /* If input is junk, use 00 as placeholder value */
                byte cellValue = 0;
                try {
                    cellValue = byte.Parse(heightsGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                } catch { }

                /* Change value in matrix object */
                currentMatrix.altitudes[e.RowIndex, e.ColumnIndex] = cellValue;
            }
        }

        private void widthUpDown_ValueChanged(object sender, EventArgs e) {
            if (parent.disableHandlers) {
                return;
            }
            parent.disableHandlers = true;

            /* Add or remove rows in DataGridView control */
            int delta = (int)widthUpDown.Value - currentMatrix.width;
            for (int i = 0; i < Math.Abs(delta); i++) {
                if (delta < 0) {
                    headersGridView.Columns.RemoveAt(currentMatrix.width - 1 - i);
                    heightsGridView.Columns.RemoveAt(currentMatrix.width - 1 - i);
                    mapFilesGridView.Columns.RemoveAt(currentMatrix.width - 1 - i);
                } else {
                    /* Add columns */
                    int index = currentMatrix.width + i;
                    headersGridView.Columns.Add(" ", (index).ToString());
                    heightsGridView.Columns.Add(" ", (index).ToString());
                    mapFilesGridView.Columns.Add(" ", (index).ToString());

                    /* Adjust column width */
                    headersGridView.Columns[index].Width = 34;
                    heightsGridView.Columns[index].Width = 22;
                    mapFilesGridView.Columns[index].Width = 34;

                    /* Fill new rows */
                    for (int j = 0; j < currentMatrix.height; j++) {
                        headersGridView.Rows[j].Cells[index].Value = 0;
                        heightsGridView.Rows[j].Cells[index].Value = 0;
                        mapFilesGridView.Rows[j].Cells[index].Value = GameMatrix.EMPTY;
                    }
                }
            }

            /* Modify matrix object */
            currentMatrix.ResizeMatrix((int)heightUpDown.Value, (int)widthUpDown.Value);
            parent.disableHandlers = false;
        }

        private void heightUpDown_ValueChanged(object sender, EventArgs e) {
            if (parent.disableHandlers) {
                return;
            }
            parent.disableHandlers = true;

            /* Add or remove rows in DataGridView control */
            int delta = (int)heightUpDown.Value - currentMatrix.height;
            for (int i = 0; i < Math.Abs(delta); i++) {
                if (delta < 0) { // Remove rows
                    headersGridView.Rows.RemoveAt(currentMatrix.height - 1 - i);
                    heightsGridView.Rows.RemoveAt(currentMatrix.height - 1 - i);
                    mapFilesGridView.Rows.RemoveAt(currentMatrix.height - 1 - i);
                } else {
                    /* Add row in DataGridView */
                    headersGridView.Rows.Add();
                    heightsGridView.Rows.Add();
                    mapFilesGridView.Rows.Add();

                    int index = currentMatrix.height + i;
                    headersGridView.Rows[index].HeaderCell.Value = (index).ToString();
                    heightsGridView.Rows[index].HeaderCell.Value = (index).ToString();
                    mapFilesGridView.Rows[index].HeaderCell.Value = (index).ToString();

                    /* Fill new rows */
                    for (int j = 0; j < currentMatrix.width; j++) {
                        headersGridView.Rows[index].Cells[j].Value = 0;
                        heightsGridView.Rows[index].Cells[j].Value = 0;
                        mapFilesGridView.Rows[index].Cells[j].Value = GameMatrix.EMPTY;
                    }
                }
            }

            /* Modify matrix object */
            currentMatrix.ResizeMatrix((int)heightUpDown.Value, (int)widthUpDown.Value);
            parent.disableHandlers = false;
        }

        private void heightsGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (e.Value is null) {
                return;
            }

            parent.disableHandlers = true;

            /* Format table cells corresponding to border maps or void */
            ushort colorValue = 0;
            try {
                colorValue = ushort.Parse(mapFilesGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            } catch { }

            (Color back, Color fore) = FormatMapCell(colorValue);
            e.CellStyle.BackColor = back;
            e.CellStyle.ForeColor = fore;

            /* If invalid input is entered, show 00 */
            byte cellValue = 0;
            try {
                cellValue = byte.Parse(heightsGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            } catch { }

            e.Value = cellValue;
            parent.disableHandlers = false;
        }

        private void importMatrixButton_Click(object sender, EventArgs e) {
            /* Prompt user to select .mtx file */
            if (selectMatrixComboBox.SelectedIndex == 0) {
                parent.statusLabelMessage("Awaiting user response...");
                DialogResult d = MessageBox.Show("Replacing a matrix - especially Matrix 0 - with a new file is risky.\n" +
                    "Do not do it unless you are absolutely sure.\nProceed?", "Risky operation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (d == DialogResult.No) {
                    return;
                }
            }

            OpenFileDialog importMatrix = new OpenFileDialog {
                Filter = GameMatrix.DefaultFilter
            };
            if (importMatrix.ShowDialog(this) != DialogResult.OK) {
                return;
            }

            /* Update matrix object in memory */
            currentMatrix = new GameMatrix(new FileStream(importMatrix.FileName, FileMode.Open));

            /* Refresh DataGridView tables */
            ClearMatrixTables();
            GenerateMatrixTables();

            /* Setup matrix editor controls */
            parent.disableHandlers = true;
            matrixNameTextBox.Text = currentMatrix.name;
            widthUpDown.Value = currentMatrix.width;
            heightUpDown.Value = currentMatrix.height;
            parent.disableHandlers = false;

            /* Display success message */
            MessageBox.Show("Matrix imported successfully!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            parent.statusLabelMessage();
        }

        private void mapFilesGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) {
                if (currentMatrix.maps[e.RowIndex, e.ColumnIndex] == GameMatrix.EMPTY) {
                    MessageBox.Show("You can't load an empty map.\nSelect a valid map and try again.\n\n" +
                        "If you only meant to change the value of this cell, wait some time between one mouse click and the other.\n" +
                        "Alternatively, highlight the cell and press F2 on your keyboard.",
                        "User attempted to load VOID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!parent.mapEditorIsReady) {
                    parent.SetupMapEditor();
                }

                int mapCount = parent.GetRomInfo().GetMapCount();
                if (currentMatrix.maps[e.RowIndex, e.ColumnIndex] >= mapCount) {
                    MessageBox.Show("This matrix cell points to a map file that doesn't exist.",
                        "There " + ((mapCount > 1) ? "are only " + mapCount + " map files." : "is only 1 map file."), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                /* Determine area data */
                ushort headerID = 0;
                if (currentMatrix.hasHeadersSection) {
                    headerID = currentMatrix.headers[e.RowIndex, e.ColumnIndex];
                } else {
                    ushort[] result = HeaderSearch.AdvancedSearch(0, (ushort)parent.internalNames.Count, parent.internalNames, (int)MapHeader.SearchableFields.MatrixID, (int)HeaderSearch.NumOperators.Equal, selectMatrixComboBox.SelectedIndex.ToString())
                        .Select(x => ushort.Parse(x.Split()[0]))
                        .ToArray();

                    if (result.Length < 1) {
                        headerID = parent.currentHeader.ID;
                        parent.statusLabelMessage("This Matrix is not linked to any Header. DSPRE can't determine the most appropriate AreaData (and textures) to use.\nDisplaying Textures from the last selected Header (" + headerID + ")'s AreaData...");
                    } else {
                        if (result.Length > 1) {
                            if (result.Contains(parent.currentHeader.ID)) {
                                headerID = parent.currentHeader.ID;

                                parent.statusLabelMessage("Multiple Headers are associated to this Matrix, including the last selected one [Header " + headerID + "]. Now using its textures.");
                            } else {
                                if (gameFamily.Equals(gFamEnum.DP)) {
                                    foreach (ushort r in result) {
                                        HeaderDP hdp;
                                        hdp = (HeaderDP)MapHeader.LoadFromARM9(r);

                                        if (hdp.locationName != 0) {
                                            headerID = hdp.ID;
                                            break;
                                        }
                                    }
                                } else if (gameFamily.Equals(gFamEnum.Plat)) {
                                    foreach (ushort r in result) {
                                        HeaderPt hpt;
                                        if (ROMToolboxDialog.flag_DynamicHeadersPatchApplied || ROMToolboxDialog.CheckFilesDynamicHeadersPatchApplied()) {
                                            hpt = (HeaderPt)MapHeader.LoadFromFile(RomInfo.gameDirs[DirNames.dynamicHeaders].unpackedDir + "\\" + r.ToString("D4"), r, 0);
                                        } else {
                                            hpt = (HeaderPt)MapHeader.LoadFromARM9(r);
                                        }

                                        if (hpt.locationName != 0) {
                                            headerID = hpt.ID;
                                            break;
                                        }
                                    }
                                } else {
                                    foreach (ushort r in result) {
                                        HeaderHGSS hgss;
                                        if (ROMToolboxDialog.flag_DynamicHeadersPatchApplied || ROMToolboxDialog.CheckFilesDynamicHeadersPatchApplied()) {
                                            hgss = (HeaderHGSS)MapHeader.LoadFromFile(RomInfo.gameDirs[DirNames.dynamicHeaders].unpackedDir + "\\" + r.ToString("D4"), r, 0);
                                        } else {
                                            hgss = (HeaderHGSS)MapHeader.LoadFromARM9(r);
                                        }

                                        if (hgss.locationName != 0) {
                                            headerID = hgss.ID;
                                            break;
                                        }
                                    }
                                }

                                parent.statusLabelMessage("Multiple Headers are using this Matrix. Header " + headerID + "'s textures are currently being displayed.");
                            }
                        } else {
                            headerID = result[0];
                            parent.statusLabelMessage("Loading Header " + headerID + "'s textures.");
                        }
                    }
                }
                Update();

                if (headerID > parent.internalNames.Count) {
                    MessageBox.Show("This map is associated to a non-existent header.\nThis will lead to unpredictable behaviour and, possibily, problems, if you attempt to load it in game.",
                        "Invalid header", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    headerID = 0;
                }

                /* get texture file numbers from area data */
                MapHeader h;
                if (ROMToolboxDialog.flag_DynamicHeadersPatchApplied || ROMToolboxDialog.CheckFilesDynamicHeadersPatchApplied()) {
                    h = MapHeader.LoadFromFile(RomInfo.gameDirs[DirNames.dynamicHeaders].unpackedDir + "\\" + headerID.ToString("D4"), headerID, 0);
                } else {
                    h = MapHeader.LoadFromARM9(headerID);
                }

                /* Load Map File and switch to Map Editor tab */
                AreaData areaData = new AreaData(h.areaDataID);
                parent.SetUpAreaData(areaData, e.RowIndex, e.ColumnIndex);
                parent.ChangeTab("Map");
            }
        }

        private void mapFilesGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            if (parent.disableHandlers) {
                return;
            }
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) {
                /* If input is junk, use '\' (FF FF) as placeholder value */
                ushort cellValue = GameMatrix.EMPTY;
                try {
                    cellValue = ushort.Parse(mapFilesGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                } catch { }

                /* Change value in matrix object */
                currentMatrix.maps[e.RowIndex, e.ColumnIndex] = cellValue;
            }
        }

        private void mapFilesGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            parent.disableHandlers = true;

            /* Format table cells corresponding to border maps or void */
            ushort colorValue = GameMatrix.EMPTY;
            try {
                colorValue = ushort.Parse(mapFilesGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            } catch { }

            (Color backColor, Color foreColor) cellColors = FormatMapCell(colorValue);
            e.CellStyle.BackColor = cellColors.backColor;
            e.CellStyle.ForeColor = cellColors.foreColor;

            if (colorValue == GameMatrix.EMPTY)
                e.Value = '-';

            parent.disableHandlers = false;
        }

        private void matrixNameTextBox_TextChanged(object sender, EventArgs e) {
            if (parent.disableHandlers) {
                return;
            }
            currentMatrix.name = matrixNameTextBox.Text;
        }

        private void removeHeadersButton_Click(object sender, EventArgs e) {
            matrixTabControl.TabPages.Remove(headersTabPage);
            currentMatrix.hasHeadersSection = false;
        }

        private void removeHeightsButton_Click(object sender, EventArgs e) {
            matrixTabControl.TabPages.Remove(heightsTabPage);
            currentMatrix.hasHeightsSection = false;
        }

        private void removeMatrixButton_Click(object sender, EventArgs e) {
            if (selectMatrixComboBox.Items.Count > 1) {
                DialogResult d = MessageBox.Show("Are you sure you want to delete the last matrix?", "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (d.Equals(DialogResult.Yes)) {
                    /* Delete matrix file */
                    int matrixToDelete = parent.GetRomInfo().GetMatrixCount() - 1;

                    string matrixPath = RomInfo.gameDirs[DirNames.matrices].unpackedDir + "\\" + matrixToDelete.ToString("D4");
                    File.Delete(matrixPath);

                    /* Change selected index if the matrix to be deleted is currently selected */
                    if (selectMatrixComboBox.SelectedIndex == matrixToDelete) {
                        selectMatrixComboBox.SelectedIndex--;
                    }

                    if (parent.eventEditorIsReady) {
                        (parent.GetEditor("Event") as EventEditor).Modify_eventMatrixUpDown_Max(false);
                    }

                    /* Remove entry from ComboBox, and decrease matrix count */
                    selectMatrixComboBox.Items.RemoveAt(matrixToDelete);
                }
            } else {
                MessageBox.Show("At least one matrix must be kept.", "Can't delete Matrix", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void setSpawnPointButton_Click(object sender, EventArgs e) {
            DataGridViewCell selectedCell = null;
            switch (matrixTabControl.SelectedIndex) {
                case 0: //Maps
                    selectedCell = mapFilesGridView.SelectedCells[0];
                    selectedCell = headersGridView.Rows[selectedCell.RowIndex].Cells[selectedCell.ColumnIndex];
                    break;

                case 1: //Headers
                    selectedCell = headersGridView.SelectedCells[0];
                    break;

                case 2: //Altitudes
                    selectedCell = heightsGridView.SelectedCells[0];
                    selectedCell = headersGridView.Rows[selectedCell.RowIndex].Cells[selectedCell.ColumnIndex];
                    break;
            }

            ushort headerNumber = 0;
            HashSet<string> result = null;
            if (currentMatrix.hasHeadersSection) {
                headerNumber = Convert.ToUInt16(selectedCell.Value);
            } else {
                DialogResult d;
                d = MessageBox.Show("This Matrix doesn't have a Header Tab. " +
                    Environment.NewLine + "Do you want to check if any Header uses this Matrix and choose that one as your Spawn Header? " +
                    Environment.NewLine + "\nChoosing 'No' will pick the last selected Header.", "Couldn't find Header Tab", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (d == DialogResult.Yes) {
                    result = HeaderSearch.AdvancedSearch(0, (ushort)parent.internalNames.Count, parent.internalNames, (int)MapHeader.SearchableFields.MatrixID, (int)HeaderSearch.NumOperators.Equal, selectMatrixComboBox.SelectedIndex.ToString());
                    if (result.Count < 1) {
                        MessageBox.Show("The current Matrix isn't assigned to any Header.\nThe default choice has been set to the last selected Header.", "No result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        headerNumber = parent.currentHeader.ID;
                    } else if (result.Count == 1) {
                        headerNumber = ushort.Parse(result.First().Split()[0]);
                    } else {
                        MessageBox.Show("Multiple Headers are using this Matrix.\nPick one from the list or reset the filter results to choose a different Header.", "Multiple results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                } else {
                    headerNumber = parent.currentHeader.ID;
                }
            }

            int matrixX = selectedCell.ColumnIndex;
            int matrixY = selectedCell.RowIndex;

            using (SpawnEditor ed = new SpawnEditor(result, parent.headerListBoxNames, headerNumber, matrixX, matrixY)) {
                ed.ShowDialog();
            }
        }

        private void selectMatrixComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (parent.disableHandlers) {
                return;
            }
            ClearMatrixTables();
            currentMatrix = new GameMatrix(selectMatrixComboBox.SelectedIndex);
            GenerateMatrixTables();

            /* Setup matrix editor controls */
            parent.disableHandlers = true;
            matrixNameTextBox.Text = currentMatrix.name;
            widthUpDown.Value = currentMatrix.width;
            heightUpDown.Value = currentMatrix.height;
            parent.disableHandlers = false;
        }

        private void importColorTableButton_Click(object sender, EventArgs e) {
            OpenFileDialog of = new OpenFileDialog {
                Filter = "DSPRE Color Table File (*.ctb)|*.ctb"
            };
            if (of.ShowDialog(this) != DialogResult.OK) {
                return;
            }

            ReadColorTable(of.FileName, silent: false);
        }

        private void resetColorTableButton_Click(object sender, EventArgs e) {
            parent.GetRomInfo().ResetMapCellsColorDictionary();
            ClearMatrixTables();
            GenerateMatrixTables();

            Properties.Settings.Default.lastColorTablePath = "";
        }

        /*
        private void ExportAllMovePermissionsInMatrix(object sender, EventArgs e) {
            CommonOpenFileDialog romFolder = new CommonOpenFileDialog();
            romFolder.IsFolderPicker = true;
            romFolder.Multiselect = false;

            if (romFolder.ShowDialog() != CommonFileDialogResult.Ok) {
                return;
            }

            for (int i = 0; i < currentMatrix.height; i++) {
                for (int j = 0; j < currentMatrix.width; j++) {
                    ushort val = currentMatrix.maps[i, j];
                    if (val < ushort.MaxValue) {
                        string path = romFolder.FileName + "\\" + currentMatrix.id + j.ToString("D2") + "_" + i.ToString("D2") + ".per";
                        File.WriteAllBytes(path, new MapFile(val).CollisionsToByteArray());
                    }
                }
            }
        }
        */
    }
}