﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibNDSFormats.NSBMD;
using LibNDSFormats.NSBTX;
using Tao.OpenGl;

namespace DS_Map
{
    public partial class BuildingEditor : Form
    {
        #region Variables
        string folder;
        bool disableHandlers = new bool();
        RomInfo info;
        NSBMD currentNSBMD;
        NSBMDGlRenderer renderer = new NSBMDGlRenderer();
        
        public static float ang = 0.0f;
        public static float dist = 12.8f;
        public static float elev = 50.0f;
        public static float tempAng = 0.0f;
        public static float tempDist = 0.0f;
        public static float tempElev = 0.0f;
        public float perspective = 45f;
        #endregion

        public BuildingEditor(RomInfo romInfo)
        {
            InitializeComponent();
            info = romInfo;

            buildingOpenGLControl.InitializeContexts();
            buildingOpenGLControl.MakeCurrent();
            buildingOpenGLControl.MouseWheel += new MouseEventHandler(buildingOpenGLControl_MouseWheel);
            Gl.glEnable(Gl.GL_TEXTURE_2D);

            if (romInfo.GetVersion() == "HeartGold" || romInfo.GetVersion() == "SoulSilver") interiorCheckBox.Enabled = true;

            disableHandlers = true;
            FillListBox(false);
            FillTexturesBox();
            textureComboBox.SelectedIndex = 0;
            disableHandlers = false;
            buildingsListBox.SelectedIndex = 0;            
        }

        #region Subroutines
        private void CreateEmbeddedTexturesFile(int modelID, bool interior)
        {
            string readingPath = folder + info.GetBuildingModelsFolderPath(interior) + "\\" + modelID.ToString("D4");
            string writingPath = Path.GetTempPath() + "BLDtexture.nsbtx";

            using (BinaryReader reader = new BinaryReader(new FileStream(readingPath, FileMode.Open)))
            {
                BinaryWriter writer = new BinaryWriter(new FileStream(writingPath, FileMode.Create));

                reader.BaseStream.Position = 0x8;
                int nsbmdSize = reader.ReadInt32(); // Read size of NSBMD file
                reader.BaseStream.Position = 0x14;
                int texturesOffset = reader.ReadInt32(); // Read starting offset of embedded textures sections
                int texturesSize = nsbmdSize - texturesOffset + 0x14; // Calculate size of embedded textures section
                reader.BaseStream.Position = texturesOffset;

                writer.Write((UInt32)0x30585442); // Write magic code BTX0
                writer.Write((UInt32)0x0001FEFF); // Sequence needed after BTX0
                writer.Write((UInt32)texturesSize); // Write size of textures block
                writer.Write((UInt32)0x00010010); // Needed sequence
                writer.Write((UInt32)0x00000014); // Needed sequence
                while (reader.BaseStream.Position < reader.BaseStream.Length) writer.Write(reader.ReadByte()); // Write texture data to file

                writer.Close();
            }


        }
        private void FillListBox(bool interior)
        {
            int modelCount = Directory.GetFiles(folder + info.GetBuildingModelsFolderPath(interior)).Length;
            for (int i = 0; i < modelCount; i++)
            {
                using (BinaryReader reader = new BinaryReader(File.OpenRead(folder + info.GetBuildingModelsFolderPath(interior) + "\\" + i.ToString("D4"))))
                {
                    reader.BaseStream.Position = 0x14;
                    if (reader.ReadUInt32() == 0x304C444D) reader.BaseStream.Position = 0x34;
                    else reader.BaseStream.Position = 0x38;
                    string nsbmdName = Encoding.UTF8.GetString(reader.ReadBytes(16));
                    buildingsListBox.Items.Add(i + ": " + nsbmdName);
                }
            }
        }
        private void FillTexturesBox()
        {
            int texturesCount = Directory.GetFiles(folder + info.GetBuildingTexturesFolderPath()).Length;
            textureComboBox.Items.Add("Embedded textures");
            for (int i = 0; i < texturesCount; i++) textureComboBox.Items.Add("Texture " + i);
        }
        private void LoadBuildingModel(int modelID, bool interior)
        {
            string path = folder + info.GetBuildingModelsFolderPath(interior) + "\\" + modelID.ToString("D4");
            using (Stream fs = new FileStream(path, FileMode.Open)) currentNSBMD = NSBMDLoader.LoadNSBMD(fs);
        }
        private void LoadModelTextures(int fileID)
        {
            string path;
            if (fileID > -1) path = folder + info.GetBuildingTexturesFolderPath() + "\\" + fileID.ToString("D4");
            else path = Path.GetTempPath() + "BLDtexture.nsbtx"; // Load Embedded textures if the argument passed to this function is -1
            try
            {
                currentNSBMD.materials = NSBTXLoader.LoadNsbtx(new MemoryStream(File.ReadAllBytes(path)), out currentNSBMD.Textures, out currentNSBMD.Palettes);
                currentNSBMD.MatchTextures();
            }
            catch { }
        }
        private void RenderModel()
        {
            MKDS_Course_Editor.NSBTA.NSBTA.NSBTA_File ani = new MKDS_Course_Editor.NSBTA.NSBTA.NSBTA_File();
            MKDS_Course_Editor.NSBTP.NSBTP.NSBTP_File tp = new MKDS_Course_Editor.NSBTP.NSBTP.NSBTP_File();
            MKDS_Course_Editor.NSBCA.NSBCA.NSBCA_File ca = new MKDS_Course_Editor.NSBCA.NSBCA.NSBCA_File();
            int[] aniframeS = new int[0];

            buildingOpenGLControl.Invalidate(); // Invalidate drawing surface
            SetupRenderer(ang, dist, elev, perspective); // Adjust rendering settings

            /* Render the building model */
            renderer.Model = currentNSBMD.models[0];
            Gl.glScalef(currentNSBMD.models[0].modelScale / 32, currentNSBMD.models[0].modelScale / 32, currentNSBMD.models[0].modelScale / 32);
            renderer.RenderModel("", ani, aniframeS, aniframeS, aniframeS, aniframeS, aniframeS, ca, false, -1, 0.0f, 0.0f, dist, elev, ang, true, tp, currentNSBMD);
        }
        private void SetupRenderer(float ang, float dist, float elev, float perspective)
        {
            Gl.glEnable(Gl.GL_RESCALE_NORMAL);
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_NORMALIZE);
            Gl.glDisable(Gl.GL_CULL_FACE);
            Gl.glFrontFace(Gl.GL_CCW);
            Gl.glClearDepth(1);
            Gl.glEnable(Gl.GL_ALPHA_TEST);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glAlphaFunc(Gl.GL_GREATER, 0f);
            Gl.glClearColor(51f / 255f, 51f / 255f, 51f / 255f, 1f);
            float aspect;
            Gl.glViewport(0, 0, buildingOpenGLControl.Width, buildingOpenGLControl.Height);
            aspect = buildingOpenGLControl.Width / buildingOpenGLControl.Height;//(vp[2] - vp[0]) / (vp[3] - vp[1]);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(perspective, aspect, 0.02f, 1000000.0f);//0.02f, 32.0f);
            Gl.glTranslatef(0, 0, -dist);
            Gl.glRotatef(elev, 1, 0, 0);
            Gl.glRotatef(ang, 0, 1, 0);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glTranslatef(0, 0, -dist);
            Gl.glRotatef(elev, 1, 0, 0);
            Gl.glRotatef(-ang, 0, 1, 0);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, new float[] { 1, 1, 1, 0 });
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, new float[] { 1, 1, 1, 0 });
            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_POSITION, new float[] { 1, 1, 1, 0 });
            Gl.glLightfv(Gl.GL_LIGHT3, Gl.GL_POSITION, new float[] { 1, 1, 1, 0 });
            Gl.glLoadIdentity();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glColor3f(1.0f, 1.0f, 1.0f);
            Gl.glDepthMask(Gl.GL_TRUE);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
        }
        #endregion

        private void buildingOpenGLControl_MouseWheel(object sender, MouseEventArgs e) // Zoom In/Out
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift) dist += (float)e.Delta / 200;
            else dist -= (float)e.Delta / 200;
            RenderModel();
        }
        private void buildingsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (disableHandlers) return;

            LoadBuildingModel(buildingsListBox.SelectedIndex, interiorCheckBox.Checked);
            CreateEmbeddedTexturesFile(buildingsListBox.SelectedIndex, interiorCheckBox.Checked);
            LoadModelTextures(textureComboBox.SelectedIndex - 1);
            RenderModel();
        }
        private void exportButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog em = new SaveFileDialog();
            em.Filter = "NSBMD model (*.nsbmd)|*.nsbmd";
            if (em.ShowDialog(this) != DialogResult.OK)
                return;

            else File.Copy(folder + info.GetBuildingModelsFolderPath(interiorCheckBox.Checked) + "\\" + buildingsListBox.SelectedIndex.ToString("D4"), em.FileName, true);
        }
        private void importButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog im = new OpenFileDialog();
            im.Filter = "NSBMD model (*.nsbmd)|*.nsbmd";
            if (im.ShowDialog(this) != DialogResult.OK)
                return;

            using (BinaryReader reader = new BinaryReader(new FileStream(im.FileName, FileMode.Open)))
            {
                if (reader.ReadUInt32() != 0x30444D42)
                {
                    MessageBox.Show("Please select an NSBMD file.", "Invalid File");
                    return;
                }
                else
                {
                    File.Copy(im.FileName, folder + info.GetBuildingModelsFolderPath(interiorCheckBox.Checked) + "\\" + buildingsListBox.SelectedIndex.ToString("D4"), true);
                    buildingsListBox_SelectedIndexChanged(null, null);
                }                
            }
        }
        private void interiorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            disableHandlers = true;

            buildingsListBox.Items.Clear();
            FillListBox(interiorCheckBox.Checked);

            disableHandlers = false;

            buildingsListBox.SelectedIndex = 0;
        }
        private void textureComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (disableHandlers) return;
            LoadModelTextures(textureComboBox.SelectedIndex - 1);
            RenderModel();
        }

        private void buildingOpenGLControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    ang += 1;
                    break;
                case Keys.Left:
                    ang -= 1;
                    break;
                case Keys.Down:
                    elev += 1;
                    break;
                case Keys.Up:
                    elev -= 1;
                    break;
            }
            RenderModel();
        }
    }
}
