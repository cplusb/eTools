using eTools.Datas;
using HZH_Controls.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace eTools.HZ_Blueprints
{
    partial class BlueprintsMindMapping
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ucMindMappingPanel1 = new HZH_Controls.Controls.UCMindMappingPanel();
            this.SuspendLayout();
            // 
            // ucMindMappingPanel1
            // 
            this.ucMindMappingPanel1.AutoScroll = true;
            this.ucMindMappingPanel1.BackColor = Color.White;
            this.ucMindMappingPanel1.DataSource = null;
            this.ucMindMappingPanel1.Dock = DockStyle.Fill;
            this.ucMindMappingPanel1.ItemBackcolor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ucMindMappingPanel1.ItemContextMenuStrip = null;
            this.ucMindMappingPanel1.LineColor = Color.Black;
            this.ucMindMappingPanel1.Location = new System.Drawing.Point(0, 0);
            this.ucMindMappingPanel1.Size = new System.Drawing.Size(707, 581);
            this.ucMindMappingPanel1.Size = new System.Drawing.Size(1920, 1080);
            this.ucMindMappingPanel1.TabIndex = 1;
            // 
            // BlueprintsMindMapping
            // 
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.White;
            this.Controls.Add(this.ucMindMappingPanel1);
            this.Name = "BlueprintsMindMapping";
            this.Size = new System.Drawing.Size(707, 581);
            this.Load += new EventHandler(this.BlueprintsMindMapping_Load);
            this.ResumeLayout(false);
        }
        #endregion

        private UCMindMappingPanel ucMindMappingPanel1;
    }

    [ToolboxItem(false)]
    public partial class BlueprintsMindMapping : UserControl
    {
        public BlueprintsMindMapping()
        {
             InitializeComponent();
        }
        private void BlueprintsMindMapping_Load(object sender, EventArgs e)
        {

        }
        public void ShowItem(BlueprintsTreeNode node)
        {
            try
            {
                MindMappingItemEntity entity = new MindMappingItemEntity()
                {
                    ID = node.name,
                    Text = $"{node.name}: {node.count}",
                    ForeColor = Color.White,
                    IsExpansion = true
                };
                entity.Childrens = Search(node);
                this.ucMindMappingPanel1.DataSource = entity;
            }
            catch (Exception exc)
            {
                System.Windows.MessageBox.Show(exc.ToString(), "错误");
            }
        }

        private MindMappingItemEntity[] Search(BlueprintsTreeNode tnode)
        {
            MindMappingItemEntity[] cs = new MindMappingItemEntity[tnode.Nodes.Count];
            for(var i = 0; i<tnode.Nodes.Count; i++)
            {
                var node = tnode.Nodes[i];
                if(node != null)
                {
                    cs[i] = new MindMappingItemEntity()
                    {
                        ID = node.name,
                        Text = $"{node.name}: {node.count}",
                        BackColor = Color.Green,
                        ForeColor = Color.White,
                        IsExpansion = true
                    };
                    cs[i].IsExpansion = true;
                    if (node.Nodes.Count > 0)
                        cs[i].Childrens = Search(node);
                }
            }
            return cs;
        }
    }

    public class BlueprintsShowManager
    {
        private static BlueprintsMindMapping _mindMapping;
        static BlueprintsShowManager() => _mindMapping = new BlueprintsMindMapping() { Dock = DockStyle.Fill };

        public static void Show(Panel panelCtrl, BlueprintsTreeNode node)
        {
            panelCtrl.Controls.Clear();
            panelCtrl.Controls.Add(_mindMapping);
            _mindMapping.ShowItem(node);
        }
    }
}
