using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Datas
{
    public class NpcCorporations
    {
        public List<int> allowedMemberRaces { get; set; }
        public int ceoID { get; set; }
        public Dictionary<int, double> corporationTrades { get; set; }
        public bool deleted { get; set; }
        public Multilingual descriptionID { get; set; }
        public Dictionary<int, NpcCorporationsDivisions> divisions { get; set; }
        public int enemyID { get; set; }
        public Dictionary<int, float> exchangeRates { get; set; }
        public string extent { get; set; }
        public int factionID { get; set; }
        public int friendID { get; set; }
        public bool hasPlayerPersonnelManager { get; set; }
        public int iconID { get; set; }
        public int initialPrice { get; set; }
        public Dictionary<int, int> investors { get; set; }
        public List<int> lpOfferTables { get; set; }
        public int mainActivityID { get; set; }
        public int memberLimit { get; set; }
        public float minSecurity { get; set; }
        public int minimumJoinStanding { get; set; }
        public Multilingual nameID { get; set; }
        public int publicShares { get; set; }
        public int raceID { get; set; }
        public int secondaryActivityID { get; set; }
        public bool sendCharTerminationMessage { get; set; }
        public Int64 shares { get; set; }
        public string size { get; set; }
        public float sizeFactor { get; set; }
        public int solarSystemID { get; set; }
        public int stationID { get; set; }
        public float taxRate { get; set; }
        public string tickerName { get; set; }
        public bool uniqueName { get; set; }
        public string url { get; set; }
    }

    public class NewNpcCorporations
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool isGroup { get; set; }
        public List<NewNpcCorporations> subs { get; set; }

        public NewNpcCorporations() { }
        public NewNpcCorporations(int id, string name)
        {
            this.id = id;
            this.name = name;
            isGroup = false;
        }
        public void AddSub(NewNpcCorporations sub)
        {
            isGroup = true;
            if (subs == null)
                subs = new List<NewNpcCorporations>();
            subs.Add(sub);
        }
    }

    public class NpcCorporationsDivisions
    {
        public int divisionNumber { get; set; }
        public int leaderID { get; set; }
        public int size { get; set; }
    }
}

/*
 加达里合众国
CBD社团
疾速快递
耶利集团
三岛集团
深核矿业公司
波斯矿业股份
矿钻集团
加达里后勤部
卡拉吉塔集团
维克米集团
倒顶集团
速装工业
佩克昂集团
加达里钢铁集团
载诺集团
纳基维集团
梯队娱乐公司
异株湖集团
莱戴集团
无过载研究公司
推进动力集团
行家物流
CBD销售部
斯库维塔集团
加达里建筑集团
行家住宅公司
加达里无限基金集团
国家星域银行
现代金融
CEO合作组织
商业俱乐部
加达里商业法庭
国家档案所
加达里海军
内安局
莱戴护卫服务公司
异株湖安全部队
家庭保安公司
共和秩序局
空间航线巡管局
维克米和平工业
联合警备队
应用知识学院
科学及贸易学会
国立军事学院
合众国护卫军
--------------
盖伦特联邦
泛星船运
联邦货运局
短途物流
物资集团
星形矿业
复合收割
酷菲集团
西顿工业
莱登船业
阿洛特工业
波特克集团
促进工业
自尊集团
视角传媒
切玛科技
度沃勒实验室
飞马集团
埃拉斯多集团
盖伦特国家银行
彭德保险
加罗恩投资银行
凯勒大学
总统
参议院
联邦最高法院
联邦政府部
联邦海军
联邦情报处
联邦海关局
学院教育
高级教育中心
联邦防务联合会
--------------
米玛塔尔共和国
赛毕斯托部族
克鲁夏部族
维洛奇亚部族
布鲁特部族
共和议会
共和舰队
共和国司法部
市政管理局
共和安全局
米玛塔尔矿业联合体
核心局面公司
无限创造
埃菲尔公司
塞克斯金发展公司
土产公司
自由扩张集团
罗述集团
共和军事学院
共和大学
帕特工学院
部族解放力量
---------------
艾玛帝国
艾玛建设
天主凝聚会
帝国总装部
维鲜集团
佐洱父子集团
高尚用具公司
杜厦铸造
HZO精炼集团
天赋植体公司
帝国运输
艾玛认证通讯社
联合收割
努图拉集团
长足食品
艾玛皇家学院
帝国议长
内政部
军务部
国土评估局
内务府
艾玛商管局
艾玛海军
宫务大臣
皇室专署集团
卡多尔家族
萨拉姆家族
柯埃佐家族
阿狄莎波家族
塔什蒙贡家族
民事法庭
神学理事会
赫迪农大学
帝国学院
帝国圣战十字第二十四军团
------------------
姐妹共济会
EVE姐妹会
粮食救济会
避难会
---------
真理会
理性思维社团
--------------
艾玛达共和国 
艾玛达舰队
艾玛达领事馆
内凡达矿业协会
----------
卡尼迪王国
卡尼迪创新集团
卡尼迪工作室
卡尼迪皇家海军
卡尼迪运输
---------------
联合矿业
外空联合矿业集团
---------------
星际快捷客运联合体
星捷运
-------------
莫德团
莫德团
-----------------
统一合作关系部(统合部)
统合关系司令部
商业安全委员会
DED
协约理事会
---------------
图克尔部落
图克尔混合企业
信任伙伴集团
-----------------
辛迪加财团
印塔基银行
印塔基商业局
印塔基空间警察局
印塔基联合财团
------------------
古斯塔斯海盗
古斯塔斯集团
古斯塔斯制造
-------------
萨沙共和国
实力派
忠实制造
---------
天蛇集团
天蛇集团
天蛇集团调查处
-------------
天使企业联合体
大天使集团
救赎天使集团
守护天使集团
主天使集团
---------
血袭者同盟
血盟骑士团
 */
