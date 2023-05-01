using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Kitware.VTK;
using Kitware.mummy.Runtime;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace VTKapp
{
    public partial class Form1 : Form
    {
        private vtkRenderWindowInteractor interactor = vtkRenderWindowInteractor.New();
        private vtkAnnotatedCubeActor cube = vtkAnnotatedCubeActor.New();
        private vtkOrientationMarkerWidget orientation = vtkOrientationMarkerWidget.New();
        private vtkUnstructuredGridReader reader = vtkUnstructuredGridReader.New();
        private vtkDataSetMapper mapper = vtkDataSetMapper.New();
        private vtkActor actor = vtkActor.New();
        private vtkTesting test = vtkTesting.New();
        private vtkUnstructuredGridGeometryFilter geometry = vtkUnstructuredGridGeometryFilter.New();
        private vtkUnstructuredGridWriter writer = vtkUnstructuredGridWriter.New();
        private vtkCamera cam = vtkCamera.New();
        private vtkDistanceWidget distancewidget = vtkDistanceWidget.New();
        private vtkAngleWidget angleWidget = vtkAngleWidget.New();
        private vtkBiDimensionalWidget bi = vtkBiDimensionalWidget.New();
        private vtkCaptionWidget cap = vtkCaptionWidget.New();
        private vtkTextActor text = vtkTextActor.New();
        private vtkTextWidget twid = vtkTextWidget.New();


        string path;
        bool VisibilityON = true;
        bool distanceON = true;
        bool angleON = true;
        bool bidimensionON = true;
        bool captionON = true;
        bool textON = true;

        public Form1()
        {
            InitializeComponent();

            BoundsandTooltips();
        }

        public void BoundsandTooltips()
        {
            //Graphics size
            renderWindowControl1.SetBounds(20, 20, 520, 520);
            WidgetBox.SetBounds(560, 20, 56, 520);
            DatasetForm.SetBounds(660, 60, 200, 400);
            ToolsForm.SetBounds(660, 60, 200, 400);
            OpenBtn.SetBounds(683, 484, 50, 50);
            SaveBtn.SetBounds(789, 484, 50, 50);
            DataSetTxt.SetBounds(697, 33, 57, 13);
            ToolsTxt.SetBounds(776, 33, 43, 13);

            //Database size
            UserLogin.SetBounds(222, 178, 371, 193);
            AdminDashboard.SetBounds(59, 12, 762, 538);
            UserRegister.SetBounds(272, 160, 342, 270);
            AdminLogin.SetBounds(222, 178, 387, 193);
            AdminForgotPassword.SetBounds(263, 200, 319, 172);
            UserForgotPassword.SetBounds(263, 200, 319, 172);

            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(RegInfo, "Fill all textboxes.");
            tooltip.SetToolTip(UserInfo, "Enter an existing username and other password.");
            tooltip.SetToolTip(AdminInfo, "Enter an existing username and other password.");
            tooltip.SetToolTip(DashInfo, "Add user: Fill all textboxes.\n\nUpdate user info: enter an existing username and fill other textboxes.\n\nDelete user: enter an existing username. ");
            tooltip.SetToolTip(OpenBtn, "Open");
            tooltip.SetToolTip(SaveBtn, "Save");
            tooltip.SetToolTip(BackgroundColorBtn, "Background color");
            tooltip.SetToolTip(FitToScreenBtn, "Fit to screen");
            tooltip.SetToolTip(RotateLbtn, "Rotate left 90°");
            tooltip.SetToolTip(RotateRbtn, "Rotate right 90°");
            tooltip.SetToolTip(ScreenshotBtn, "Screenshot");
            tooltip.SetToolTip(XviewBtn, "View X");
            tooltip.SetToolTip(YviewBtn, "View Y");
            tooltip.SetToolTip(ZviewBtn, "View Z");

            //Dataset
            MeshCb.SelectedIndex = 0;
            MeshSizeCb.SelectedIndex = 0;
            PropertyNameCbox.SelectedIndex = 0;
            PropertySizeCbox.SelectedIndex = 0;
        }
        
        
        //USER LOGIN FORM
        private void UserLoginBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection con = new OleDbConnection();
                con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=VTK database.accdb";
                con.Open();
                OleDbCommand cmd = new OleDbCommand("Select * from [User] Where Username= '" + UserUsernameTbox.Text + "' and Password='" + UserPasswordTbox.Text + "' ", con);
                OleDbDataReader sdr;
                sdr = cmd.ExecuteReader();

                if (sdr.Read())
                {
                    MessageBox.Show("Successfully login!", "Accepted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UserLogin.Visible = false;
                    renderWindowControl1.Visible = true;
                    WidgetBox.Visible = true;
                    DataSetTxt.Visible = true;
                    ToolsTxt.Visible = true;
                    DatasetForm.Visible = true;
                    OpenBtn.Visible = true;
                    SaveBtn.Visible = true;
                }
                else
                {
                    MessageBox.Show("Incorect Username/Password", "Rejected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RegistrationTxt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UserLogin.Visible = false;
            UserRegister.Visible = true;
            UserUsernameTbox.Clear();
            UserPasswordTbox.Clear();
        }

        private void AdminTxt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UserLogin.Visible = false;
            AdminLogin.Visible = true;
            UserUsernameTbox.Clear();
            UserPasswordTbox.Clear();
        }

        private void UserForgotPasswordTxt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UserLogin.Visible = false;
            UserForgotPassword.Visible = true;
            UserUsernameTbox.Clear();
            UserPasswordTbox.Clear();
        }


        //ADMIN LOGIN FORM
        private void AdminLoginBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection con = new OleDbConnection();
                con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=VTK database.accdb";
                con.Open();
                OleDbCommand cmd = new OleDbCommand("Select * from [Admin] Where Username= '" + AdminUsernameTbox.Text + "' and Password='" + AdminPasswordTbox.Text + "' ", con);
                OleDbDataReader sdr;
                sdr = cmd.ExecuteReader();

                if (sdr.Read())
                {
                    MessageBox.Show("Successfully login!", "Accepted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AdminLogin.Visible = false;
                    AdminDashboard.Visible = true;
                    Table();
                }
                else
                {
                    MessageBox.Show("Incorect Username/Password", "Rejected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }

        private void AdminForgotPasswordTxt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AdminLogin.Visible = false;
            AdminForgotPassword.Visible = true;
            AdminUsernameTbox.Clear();
            AdminPasswordTbox.Clear();
        }

        private void AdminBackBtn_Click(object sender, EventArgs e)
        {
            UserLogin.Visible = true;
            AdminLogin.Visible = false;
            AdminUsernameTbox.Clear();
            AdminPasswordTbox.Clear();
        }


        //USER REGISTRATION FORM 
        private void UserRegisterBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (RegFirstnameTbox.Text == "" || RegLastnameTbox.Text == "" || RegUsernameTbox.Text == "" || RegPasswordTbox.Text == "")
                {
                    MessageBox.Show("Fill all textboxes!", "Rejected!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                OleDbConnection con = new OleDbConnection();
                con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=VTK database.accdb";
                con.Open();
                OleDbCommand cmd = new OleDbCommand("INSERT INTO [User] ([Firstname],[Lastname],[Username],[Password]) VALUES ('" + RegFirstnameTbox.Text + "', '" + RegLastnameTbox.Text + "', '" + RegUsernameTbox.Text + "', '" + RegPasswordTbox .Text+ "')", con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record save successfully!", "Accepted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UserLogin.Visible = true;
                UserRegister.Visible = false;
                RegFirstnameTbox.Clear();
                RegLastnameTbox.Clear();
                RegUsernameTbox.Clear();
                RegPasswordTbox.Clear();
                con.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Username already exist!", "Rejected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RegBackBtn_Click(object sender, EventArgs e)
        {
            UserLogin.Visible = true;
            UserRegister.Visible = false;
            RegFirstnameTbox.Clear();
            RegLastnameTbox.Clear();
            RegUsernameTbox.Clear();
            RegPasswordTbox.Clear();
        }




        //User Forgot password FORM
        private void UserChange_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserUsernameFPtbox.Text == "" || UserPasswordFPtbox.Text == "")
                {
                    MessageBox.Show("Fill all textboxes!", "Rejected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    OleDbConnection con = new OleDbConnection();
                    con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=VTK database.accdb";
                    con.Open();
                    OleDbCommand cmd = new OleDbCommand("UPDATE [User] SET [Password] ='" + UserPasswordFPtbox.Text + "' WHERE [Username] ='" + UserUsernameFPtbox.Text + "' ", con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("You changed password successfully!", "Accepted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UserPasswordFPtbox.Clear();
                    UserUsernameFPtbox.Clear();
                    UserLogin.Visible = true;
                    UserForgotPassword.Visible = false;
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UserBackFPbtn_Click(object sender, EventArgs e)
        {
            UserPasswordFPtbox.Clear();
            UserUsernameFPtbox.Clear();
            UserLogin.Visible = true;
            UserForgotPassword.Visible = false;
        }


        //Admin Forgot password FORM
        private void AdminChange_Click(object sender, EventArgs e)
        {
            try
            {
                if (AdminUsernameFPtbox.Text == "" || AdminPasswordFPtbox.Text == "")
                {
                    MessageBox.Show("Fill all textboxes!", "Rejected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    OleDbConnection con = new OleDbConnection();
                    con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=VTK database.accdb";
                    con.Open();
                    OleDbCommand cmd = new OleDbCommand("UPDATE [Admin] SET [Password] ='" + AdminPasswordFPtbox.Text + "' WHERE [Username] ='" + AdminUsernameFPtbox.Text + "' ", con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("You changed password successfully!", "Accepted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AdminUsernameFPtbox.Clear();
                    AdminPasswordFPtbox.Clear();
                    AdminLogin.Visible = true;
                    AdminForgotPassword.Visible = false;
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AdminBackFPbtn_Click(object sender, EventArgs e)
        {
            AdminUsernameFPtbox.Clear();
            AdminPasswordFPtbox.Clear();
            AdminLogin.Visible = true;
            AdminForgotPassword.Visible = false;
        }



        //DASHBOARD FORM
        private void DashAddBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (DashFirstnameTbox.Text == "" || DashLastnameTbox.Text == "" || DashUsernameTbox.Text == "" || DashPasswordTbox.Text == "")
                {
                    MessageBox.Show("Fill all textboxes!", "Rejected!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    OleDbConnection con = new OleDbConnection();
                    con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=VTK database.accdb";
                    con.Open();
                    OleDbCommand cmd = new OleDbCommand("INSERT INTO [User] ([Firstname],[Lastname],[Username],[Password]) VALUES ('" + DashFirstnameTbox.Text + "', '" + DashLastnameTbox.Text + "', '" + DashUsernameTbox.Text + "', '" + DashPasswordTbox.Text + "')", con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record save successfully!", "Accepted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UserLogin.Visible = true;
                    UserRegister.Visible = false;
                    DashFirstnameTbox.Clear();
                    DashLastnameTbox.Clear();
                    DashUsernameTbox.Clear();
                    DashPasswordTbox.Clear();
                    Table();
                    con.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Username already exist!", "Rejected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DashEditBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (DashFirstnameTbox.Text == "" || DashLastnameTbox.Text == "" || DashUsernameTbox.Text == "" || DashPasswordTbox.Text == "")
                {
                    MessageBox.Show("Fill all textboxes!", "Rejected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                OleDbConnection con = new OleDbConnection();
                con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=VTK database.accdb";
                con.Open();
                OleDbCommand cmd = new OleDbCommand("UPDATE [User] SET [Firstname] ='" + DashFirstnameTbox.Text + "', [Lastname] ='" + DashLastnameTbox.Text + "', [Password] ='" + DashPasswordTbox.Text + "' WHERE [Username] ='" + DashUsernameTbox .Text+ "' ", con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record updated successfully!", "Accepted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DashFirstnameTbox.Clear();
                DashLastnameTbox.Clear();
                DashPasswordTbox.Clear();
                DashUsernameTbox.Clear();
                Table();
                con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DashDeleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection con = new OleDbConnection();
                con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=VTK database.accdb";
                con.Open();
                OleDbCommand cmd = new OleDbCommand("DELETE FROM [User] WHERE [Username] = '" + DashUsernameTbox.Text + "'", con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record deleted successfully", "Accepted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DashFirstnameTbox.Clear();
                DashLastnameTbox.Clear();
                DashPasswordTbox.Clear();
                DashUsernameTbox.Clear();
                Table();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Table()
        {
            try
            {
                OleDbConnection con = new OleDbConnection();
                con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=VTK database.accdb";
                con.Open();
                OleDbCommand cmd = new OleDbCommand("Select * from [User]", con);
                OleDbDataAdapter sda = new OleDbDataAdapter();
                sda.SelectCommand = cmd;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                BindingSource bs = new BindingSource();
                bs.DataSource = dt;
                DashTable.DataSource = bs;
                sda.Update(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //GRAPHICS
        private void renderWindowControl1_Load(object sender, EventArgs e)
        {
            try
            {
                vtkRenderer renderer = renderWindowControl1.RenderWindow.GetRenderers().GetFirstRenderer();
                renderer.SetBackground(0,0,0);
                vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
                interactor.SetRenderWindow(renwin);

                //Orientation widget
                cube.SetXPlusFaceText("+X");
                cube.SetXMinusFaceText("-X");
                cube.SetYPlusFaceText("+Y");
                cube.SetYMinusFaceText("-Y");
                cube.SetZPlusFaceText("+Z");
                cube.SetZMinusFaceText("-Z");
                cube.SetFaceTextScale(0.5);
                cube.GetCubeProperty().SetColor(0.5, 1, 1);
                cube.GetTextEdgesProperty().SetLineWidth(1);
                cube.GetTextEdgesProperty().SetDiffuse(0);
                cube.GetTextEdgesProperty().SetAmbient(1);
                cube.GetCubeProperty().SetColor(0.1800, 0.2800, 0.2300);

                orientation.SetOutlineColor(0, 0, 0);
                orientation.SetOrientationMarker(cube);
                orientation.SetInteractor(interactor);
                orientation.SetViewport(0.0, 0.0, 0.2, 0.2);
                orientation.On();

                renderer.ResetCamera();
                renwin.Render();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //OPEN
        private void OpenBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (DialogResult.OK == dialog.ShowDialog())
                {
                    path = dialog.FileName;
                }
                
                string root = test.GetDataRoot();
                string filepath = System.IO.Path.Combine(root, path);

                reader.SetFileName(filepath);
                reader.Update();

                mapper.SetInputConnection(reader.GetOutputPort());
                actor.SetMapper(mapper);

                vtkRenderer renderer = renderWindowControl1.RenderWindow.GetRenderers().GetFirstRenderer();
                renderer.AddActor(actor);

                renderer.ResetCamera();

                vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
                renwin.Render();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //SAVE
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            geometry.SetInputConnection(reader.GetOutputPort());
            geometry.Update();

            writer.SetInput(geometry.GetOutput());

            SaveFileDialog svd = new SaveFileDialog();
            svd.Filter = "vtk files (*.vtk)|*.vtk|All files(*.*)|*.*";
            if (svd.ShowDialog() == DialogResult.OK)
            {
                writer.SetFileName(svd.FileName);
            }
            writer.SetFileTypeToBinary();
            writer.Write();
        }

        private void DataSetTxt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DatasetForm.Visible = true;
            ToolsForm.Visible = false;
        }

        private void ToolsTxt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DatasetForm.Visible = false;
            ToolsForm.Visible = true;
        }



        //WIDGET
        private void BackgroundColorBtn_Click(object sender, EventArgs e)
        {
            vtkRenderer renderer = renderWindowControl1.RenderWindow.GetRenderers().GetFirstRenderer();
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;

            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                Color color = cd.Color;

                double R = color.R / 255.0;
                double G = color.G / 255.0;
                double B = color.B / 255.0;

                renderer.SetBackground(R, G, B);
            }
            renwin.Render();
        }

        private void FitToScreenBtn_Click(object sender, EventArgs e)
        {
            vtkRenderer renderer = renderWindowControl1.RenderWindow.GetRenderers().GetFirstRenderer();
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
            renderer.ResetCamera();
            renwin.Render();
        }

        private void RotateLbtn_Click(object sender, EventArgs e)
        {
            vtkRenderer renderer = renderWindowControl1.RenderWindow.GetRenderers().GetFirstRenderer();
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
            renderer.GetActiveCamera().Roll(90);
            renwin.Render();
        }

        private void RotateRbtn_Click(object sender, EventArgs e)
        {
            vtkRenderer renderer = renderWindowControl1.RenderWindow.GetRenderers().GetFirstRenderer();
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
            renderer.GetActiveCamera().Roll(-90);
            renwin.Render();
        }

        private void ScreenshotBtn_Click(object sender, EventArgs e)
        {
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
            vtkWindowToImageFilter w2i = vtkWindowToImageFilter.New();
            w2i.SetInput(renwin);

            vtkJPEGWriter writer = vtkJPEGWriter.New();
            writer.SetInput(w2i.GetOutput());

            SaveFileDialog svd = new SaveFileDialog();
            svd.Filter = "jpg files (*.jpg)|*.jpg|All files(*.*)|*.*";
            if (svd.ShowDialog() == DialogResult.OK)
            {

                writer.SetFileName(svd.FileName);
            }
            writer.Write();

            renwin.Render();
        }

        private void XviewBtn_Click(object sender, EventArgs e)
        {
            vtkRenderer renderer = renderWindowControl1.RenderWindow.GetRenderers().GetFirstRenderer();
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;

            cam.SetFocalPoint(0, 0, 0);
            cam.SetPosition(90, 1, 1);   
            cam.ComputeViewPlaneNormal();
            cam.SetViewUp(0, 0, 1);   
            cam.OrthogonalizeViewUp();
            renderer.SetActiveCamera(cam);
            renderer.ResetCamera();
            renwin.Render();
        }

        private void YviewBtn_Click(object sender, EventArgs e)
        {
            vtkRenderer renderer = renderWindowControl1.RenderWindow.GetRenderers().GetFirstRenderer();
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;

            cam.SetFocalPoint(0, 0, 0);
            cam.SetPosition(1, 90, 1);   
            cam.ComputeViewPlaneNormal();
            cam.SetViewUp(0, 0, 1);   
            cam.OrthogonalizeViewUp();
            renderer.SetActiveCamera(cam);
            renderer.ResetCamera();
            renwin.Render();
        }

        private void ZviewBtn_Click(object sender, EventArgs e)
        {
            vtkRenderer renderer = renderWindowControl1.RenderWindow.GetRenderers().GetFirstRenderer();
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;

            cam.SetFocalPoint(0, 0, 0);
            cam.SetPosition(1, 1, 90);   
            cam.ComputeViewPlaneNormal();
            cam.SetViewUp(0, 1, 0);   
            cam.OrthogonalizeViewUp();
            renderer.SetActiveCamera(cam);
            renderer.ResetCamera();
            renwin.Render();
        }


        //DATASET
        private void VisibilityBtn_Click(object sender, EventArgs e)
        {
            if (VisibilityON == true)                                       
            {
                actor.SetVisibility(0);
                VisibilityON = false;
                VisibilityBtn.Text = "Non visible";
            }
            else
            {
                actor.SetVisibility(1);
                VisibilityON = true;
                VisibilityBtn.Text = "Visible";
            }

            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
            renwin.Render();
        }

        private void MeshApplyBtn_Click(object sender, EventArgs e)
        {
            if (MeshCb.Text == "Surface")
            {
                actor.GetProperty().SetRepresentationToSurface();
                actor.GetProperty().EdgeVisibilityOff();
                EdgeColorBtn.Enabled = false;
            }
            else if (MeshCb.Text == "Surface with edges")
            {
                actor.GetProperty().SetRepresentationToSurface();
                actor.GetProperty().EdgeVisibilityOn();
                EdgeColorBtn.Enabled = true;
            }
            else if (MeshCb.Text == "Wireframe")
            {
                actor.GetProperty().SetRepresentationToWireframe();
                EdgeColorBtn.Enabled = false;
            }
            else if (MeshCb.Text == "Points")
            {
                actor.GetProperty().SetRepresentationToPoints();
                EdgeColorBtn.Enabled = false;
            }
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
            renwin.Render();
        }

        private void MeshSizeApplyBtn_Click(object sender, EventArgs e)
        {
            actor.GetProperty().SetPointSize(int.Parse(MeshSizeCb.Text));
            actor.GetProperty().SetLineWidth(int.Parse(MeshSizeCb.Text));
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
            renwin.Render();
        }

        private void EdgeColorBtn_Click(object sender, EventArgs e)
        {
            ColorDialog colDialog = new ColorDialog();
            if (colDialog.ShowDialog() == DialogResult.OK)
            {
                Color colorObj = colDialog.Color;

                double R = colorObj.R / 255.0;
                double G = colorObj.G / 255.0;
                double B = colorObj.B / 255.0;

                actor.GetProperty().SetEdgeColor(R, G, B);
            }
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
            renwin.Render();
        }



        //TOOLS
        private void DistanceBtn_Click(object sender, EventArgs e)
        {
            if (distanceON == true)
            {
                distancewidget.SetInteractor(interactor);
                distancewidget.CreateDefaultRepresentation();
                distancewidget.On();
                distanceON = false;
            }
            else
            {
                distancewidget.Off();
                distanceON = true;
            }
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
            renwin.Render();
        }

        private void AngleBtn_Click(object sender, EventArgs e)
        {
            if (angleON == true)
            {
                angleWidget.SetInteractor(interactor);
                angleWidget.CreateDefaultRepresentation();
                angleWidget.On();
                angleON = false;
            }
            else
            {
                angleWidget.Off();
                angleON = true;
            }
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
            renwin.Render();
        }

        private void BidimensionalBtn_Click(object sender, EventArgs e)
        {
            if (bidimensionON == true)
            {
                bi.SetInteractor(interactor);
                bi.CreateDefaultRepresentation();
                bi.On();
                bidimensionON = false;
            }
            else
            {
                bi.Off();
                bidimensionON = true;
            }
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
            renwin.Render();
        }

        private void CaptionBtn_Click(object sender, EventArgs e)
        {
            if (captionON == true)
            {
                cap.SetInteractor(interactor);
                cap.CreateDefaultRepresentation();
                cap.On();
                captionON = false;
            }
            else
            {
                cap.Off();
                captionON = true;
            }
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
            renwin.Render();
        }

        private void TextBtn_Click(object sender, EventArgs e)
        {
            if (textON == true)
            {
                text.SetInput(TextTbox.Text);
                twid.SetInteractor(interactor);
                twid.CreateDefaultRepresentation();
                twid.SetTextActor(text);
                twid.On();
                textON = false;
            }
            else
            {
                twid.Off();
                textON = true;
                TextTbox.Clear();
            }
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
            renwin.Render();
        }


        //DATASET
        private void PropertyApplyNameBtn_Click(object sender, EventArgs e)
        {
            if (PropertyNameCbox.Text == "Opacity")
            {
                actor.GetProperty().SetOpacity(float.Parse(PropertySizeCbox.Text));
            }
            else if (PropertyNameCbox.Text == "Ambient")
            {
                actor.GetProperty().SetAmbient(float.Parse(PropertySizeCbox.Text));
            }
            else if (PropertyNameCbox.Text == "Specular")
            {
                actor.GetProperty().SetSpecular(float.Parse(PropertySizeCbox.Text));
            }

            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
            renwin.Render();
        }

        private void ModelColorBtn_Click(object sender, EventArgs e)
        {
            mapper.ScalarVisibilityOff();

            ColorDialog colDialog = new ColorDialog();
            if (colDialog.ShowDialog() == DialogResult.OK)
            {
                Color colorObj = colDialog.Color;

                double R = colorObj.R / 255.0;
                double G = colorObj.G / 255.0;
                double B = colorObj.B / 255.0;

                actor.GetProperty().SetColor(R, G, B);
            }
            vtkRenderWindow renwin = renderWindowControl1.RenderWindow;
            renwin.Render();
        }

      
    }
}
