"use strict";(self["webpackChunkdbproject"]=self["webpackChunkdbproject"]||[]).push([[530],{7316:function(e,a,t){t.r(a),t.d(a,{default:function(){return k}});var l=t(3396),i=t(9242),s=t(7139);const d=e=>((0,l.dD)("data-v-4d10e96b"),e=e(),(0,l.Cn)(),e),o=d((()=>(0,l._)("div",null,"  案件管理 > 受害人信息管理",-1))),n={style:{"text-align":"center"}},m={style:{display:"inline-block","text-align":"left"}},p={class:"seaButton"},r={style:{"text-align":"center"}},c={style:{display:"inline-block","text-align":"left"}},u={class:"qianru"},h=d((()=>(0,l._)("td",null,null,-1))),y={class:"seaButton"},w={style:{"text-align":"center"}},v={style:{display:"inline-block","text-align":"left"}},f={class:"qianru"},g=d((()=>(0,l._)("td",null,null,-1))),x={class:"seaButton"};function b(e,a,t,d,b,C){const I=(0,l.up)("el-header"),_=(0,l.up)("el-text"),W=(0,l.up)("el-input"),V=(0,l.up)("el-option"),k=(0,l.up)("el-select"),U=(0,l.up)("el-button"),T=(0,l.up)("el-divider"),D=(0,l.up)("el-table-column"),$=(0,l.up)("el-table"),z=(0,l.up)("el-tab-pane"),R=(0,l.up)("el-tabs");return(0,l.wg)(),(0,l.iD)(l.HY,null,[(0,l.Wm)(I,{class:"sub-header",onMousemove:C.handleMouseMove},{default:(0,l.w5)((()=>[o])),_:1},8,["onMousemove"]),(0,l.Wm)(R,{modelValue:b.anv,"onUpdate:modelValue":a[12]||(a[12]=e=>b.anv=e),type:"border-card",style:{"margin-top":"10vh"}},{default:(0,l.w5)((()=>[(0,l.Wm)(z,{label:"查询受害人",name:"1"},{default:(0,l.w5)((()=>[(0,l._)("div",n,[(0,l._)("table",m,[(0,l._)("tr",null,[(0,l._)("td",null,[(0,l.Wm)(_,{class:"noteText",type:"primary"},{default:(0,l.w5)((()=>[(0,l.Uk)("姓名：")])),_:1}),(0,l.Wm)(W,{class:"inputBox",modelValue:b.victimName,"onUpdate:modelValue":a[0]||(a[0]=e=>b.victimName=e),placeholder:"请输入姓名",clearable:""},null,8,["modelValue"])]),(0,l._)("td",null,[(0,l.Wm)(_,{class:"noteText",type:"primary"},{default:(0,l.w5)((()=>[(0,l.Uk)("性别：")])),_:1}),(0,l.Wm)(k,{class:"inputBox",modelValue:b.victimSex,"onUpdate:modelValue":a[1]||(a[1]=e=>b.victimSex=e),placeholder:"请选择",style:{width:"235px"}},{default:(0,l.w5)((()=>[(0,l.Wm)(V,{selected:"",label:"全部性别",value:""}),(0,l.Wm)(V,{label:"男",value:"M"}),(0,l.Wm)(V,{label:"女",value:"F"})])),_:1},8,["modelValue"])])]),(0,l._)("tr",null,[(0,l._)("td",null,[(0,l.Wm)(_,{class:"noteText",type:"primary"},{default:(0,l.w5)((()=>[(0,l.Uk)("住址：")])),_:1}),(0,l.Wm)(W,{class:"inputBox",modelValue:b.victimAddress,"onUpdate:modelValue":a[2]||(a[2]=e=>b.victimAddress=e),placeholder:"请输入住址",clearable:""},null,8,["modelValue"])]),(0,l._)("td",null,[(0,l.Wm)(_,{class:"noteText",type:"primary"},{default:(0,l.w5)((()=>[(0,l.Uk)("身份证号：")])),_:1}),(0,l.Wm)(W,{class:"inputBox",modelValue:b.victimID,"onUpdate:modelValue":a[3]||(a[3]=e=>b.victimID=e),placeholder:"请输入身份证号码",oninput:"value=value.replace(/[^\\d.]/g,'')",clearable:"",maxlength:"18","show-word-limit":"",style:{width:"235px"}},null,8,["modelValue"])])])]),(0,l._)("div",p,[(0,l.Wm)(U,{type:"primary",onClick:C.fetchVictimInfo},{default:(0,l.w5)((()=>[(0,l.Uk)("查询")])),_:1},8,["onClick"])])]),(0,l.Wm)(T),(0,l._)("div",null,[b.showCase?(0,l.kq)("",!0):((0,l.wg)(),(0,l.j4)($,{key:0,data:b.chanVictimInfo,stripe:"",height:"450",onWheelPassive:a[4]||(a[4]=(0,i.iM)((()=>{}),["stop"]))},{default:(0,l.w5)((()=>[(0,l.Wm)(D,{prop:"id",label:"身份证号",sortable:"",width:"250px"}),(0,l.Wm)(D,{prop:"name",label:"姓名",sortable:"",width:"150px"}),(0,l.Wm)(D,{prop:"sex",label:"性别",sortable:"",width:"150px"}),(0,l.Wm)(D,{prop:"address",label:"住址",sortable:""}),(0,l.Wm)(D,{align:"right",width:"100px"},{header:(0,l.w5)((()=>[])),default:(0,l.w5)((e=>[(0,l.Wm)(U,{size:"small",type:"primary",onClick:a=>C.getCase(e.$index,e.row)},{default:(0,l.w5)((()=>[(0,l.Uk)("涉及案件")])),_:2},1032,["onClick"])])),_:1})])),_:1},8,["data"])),b.showCase&&b.caseInfo.length>0?((0,l.wg)(),(0,l.j4)($,{key:1,data:b.caseInfo,stripe:"",height:"450",onWheelPassive:a[5]||(a[5]=(0,i.iM)((()=>{}),["stop"]))},{default:(0,l.w5)((()=>[(0,l.Wm)(D,{prop:"id",label:"案件编号",sortable:"",width:"120px"}),(0,l.Wm)(D,{prop:"type",label:"案件类型",sortable:"",width:"120px"}),(0,l.Wm)(D,{prop:"status",label:"案件状态",sortable:"",width:"120px"}),(0,l.Wm)(D,{prop:"registerTime",label:"登记时间",sortable:""}),(0,l.Wm)(D,{prop:"address",label:"案发地点",sortable:""}),(0,l.Wm)(D,{prop:"ranking",label:"案件等级",sortable:"",width:"120px"}),(0,l.Wm)(D,{align:"center",width:"80px"},{header:(0,l.w5)((()=>[(0,l.Wm)(U,{size:"small",type:"primary",onClick:C.back},{default:(0,l.w5)((()=>[(0,l.Uk)("返回")])),_:1},8,["onClick"])])),_:1})])),_:1},8,["data"])):(0,l.kq)("",!0)])])),_:1}),(0,l.Wm)(z,{label:"新增受害人",name:"2"},{default:(0,l.w5)((()=>[(0,l._)("div",r,[(0,l._)("table",c,[(0,l._)("tr",null,[(0,l._)("td",null,[(0,l.Wm)(_,{class:"noteText",type:"primary",style:{"margin-right":"10px"}},{default:(0,l.w5)((()=>[(0,l.Uk)("案件编号:")])),_:1}),(0,l.Wm)(W,{modelValue:b.addCaseID,"onUpdate:modelValue":a[6]||(a[6]=e=>b.addCaseID=e),placeholder:"案件编号",clearable:"",maxlength:"7",oninput:"value=value.replace(/[^\\d.]/g,'')","show-word-limit":"",style:{width:"190px"}},{prepend:(0,l.w5)((()=>[(0,l._)("div",u,(0,s.zw)(b.type1),1)])),_:1},8,["modelValue"])]),(0,l._)("td",null,[(0,l.Wm)(_,{class:"noteText",type:"primary"},{default:(0,l.w5)((()=>[(0,l.Uk)("案件类型:")])),_:1}),(0,l.Wm)(k,{class:"inputBox",modelValue:b.addCaseType,"onUpdate:modelValue":a[7]||(a[7]=e=>b.addCaseType=e),placeholder:"请选择",style:{width:"235px"}},{default:(0,l.w5)((()=>[(0,l.Wm)(V,{label:"强奸",value:"强奸"}),(0,l.Wm)(V,{label:"抢劫",value:"抢劫"}),(0,l.Wm)(V,{label:"故意伤害",value:"故意伤害"}),(0,l.Wm)(V,{label:"盗窃",value:"盗窃"}),(0,l.Wm)(V,{label:"诈骗",value:"诈骗"}),(0,l.Wm)(V,{label:"谋杀",value:"谋杀"})])),_:1},8,["modelValue"])])]),(0,l._)("tr",null,[h,(0,l._)("td",null,[(0,l.Wm)(_,{class:"noteText",type:"primary",style:{"margin-left":"7px"}},{default:(0,l.w5)((()=>[(0,l.Uk)("身份证号：")])),_:1}),(0,l.Wm)(W,{class:"inputBox",modelValue:b.addVictimID,"onUpdate:modelValue":a[8]||(a[8]=e=>b.addVictimID=e),placeholder:"请输入身份证号",oninput:"value=value.replace(/[^\\d.]/g,'')",style:{"margin-left":"3px",width:"235px"},clearable:"",maxlength:"18","show-word-limit":""},null,8,["modelValue"])])])]),(0,l._)("div",y,[(0,l.Wm)(U,{type:"primary",onClick:C.addVictimInfo},{default:(0,l.w5)((()=>[(0,l.Uk)("新增受害人")])),_:1},8,["onClick"])])])])),_:1}),(0,l.Wm)(z,{label:"删除受害人",name:"3"},{default:(0,l.w5)((()=>[(0,l._)("div",w,[(0,l._)("table",v,[(0,l._)("tr",null,[(0,l._)("td",null,[(0,l.Wm)(_,{class:"noteText",type:"primary",style:{"margin-right":"10px"}},{default:(0,l.w5)((()=>[(0,l.Uk)("案件编号:")])),_:1}),(0,l.Wm)(W,{modelValue:b.delCaseID,"onUpdate:modelValue":a[9]||(a[9]=e=>b.delCaseID=e),placeholder:"案件编号",clearable:"",maxlength:"7",oninput:"value=value.replace(/[^\\d.]/g,'')","show-word-limit":"",style:{width:"190px"}},{prepend:(0,l.w5)((()=>[(0,l._)("div",f,(0,s.zw)(b.type2),1)])),_:1},8,["modelValue"])]),(0,l._)("td",null,[(0,l.Wm)(_,{class:"noteText",type:"primary"},{default:(0,l.w5)((()=>[(0,l.Uk)("案件类型:")])),_:1}),(0,l.Wm)(k,{class:"inputBox",modelValue:b.delCaseType,"onUpdate:modelValue":a[10]||(a[10]=e=>b.delCaseType=e),placeholder:"请选择",style:{width:"235px"}},{default:(0,l.w5)((()=>[(0,l.Wm)(V,{label:"强奸",value:"强奸"}),(0,l.Wm)(V,{label:"抢劫",value:"抢劫"}),(0,l.Wm)(V,{label:"故意伤害",value:"故意伤害"}),(0,l.Wm)(V,{label:"盗窃",value:"盗窃"}),(0,l.Wm)(V,{label:"诈骗",value:"诈骗"}),(0,l.Wm)(V,{label:"谋杀",value:"谋杀"})])),_:1},8,["modelValue"])])]),(0,l._)("tr",null,[g,(0,l._)("td",null,[(0,l.Wm)(_,{class:"noteText",type:"primary",style:{"margin-left":"7px"}},{default:(0,l.w5)((()=>[(0,l.Uk)("身份证号：")])),_:1}),(0,l.Wm)(W,{class:"inputBox",modelValue:b.delVictimID,"onUpdate:modelValue":a[11]||(a[11]=e=>b.delVictimID=e),placeholder:"请输入身份证号",oninput:"value=value.replace(/[^\\d.]/g,'')",style:{"margin-left":"3px",width:"235px"},clearable:"",maxlength:"18","show-word-limit":""},null,8,["modelValue"])])])]),(0,l._)("div",x,[(0,l.Wm)(U,{type:"primary",onClick:C.delVictimInfo},{default:(0,l.w5)((()=>[(0,l.Uk)("删除受害人")])),_:1},8,["onClick"])])])])),_:1})])),_:1},8,["modelValue"])],64)}var C=t(7138),I=t(7178),_={data(){return{anv:"1",showCase:!1,victimID:"",victimName:"",victimSex:"",victimAddress:"",victimInfo:[],chanVictimInfo:[],caseInfo:[],addVictimID:"",addCaseID:"",addCaseType:"",type1:"",delVictimID:"",delCaseID:"",delCaseType:"",type2:""}},methods:{handleMouseMove(e){const a=e.pageX-e.target.offsetLeft,t=e.pageY-e.target.offsetTop;e.target.style.setProperty("--x",`${a}px`),e.target.style.setProperty("--y",`${t}px`)},async fetchVictimInfo(){try{const e=await C.Z.get(`/api/victimInfo?victimID=${encodeURIComponent(this.victimID)}&victimName=${encodeURIComponent(this.victimName)}&victimSex=${encodeURIComponent(this.victimSex)}&victimAddress=${encodeURIComponent(this.victimAddress)}`);this.victimInfo=e.data,this.chanVictimInfo=this.victimInfo.map((e=>"M"===e.sex?{...e,sex:"男"}:"F"===e.sex?{...e,sex:"女"}:e))}catch(e){(0,I.z8)({showClose:!0,message:"获取受害人列表失败！",type:"error",duration:5e3})}},async getCase(e,a){try{const e=await C.Z.get(`/api/getCaseV?victimID=${encodeURIComponent(a.id)}`);for(let a=0;a<e.data.length;a++)e.data[a].registerTime=e.data[a].registerTime.replace("T"," "),"0"==e.data[a].ranking?e.data[a].ranking="一般":"1"==e.data[a].ranking?e.data[a].ranking="较大":"2"==e.data[a].ranking?e.data[a].ranking="重大":"3"==e.data[a].ranking&&(e.data[a].ranking="特别重大");this.caseInfo=e.data,this.showCase=!0}catch(t){(0,I.z8)({showClose:!0,message:"获取案件信息失败！",type:"error",duration:5e3})}},async addVictimInfo(){try{const e=await C.Z.post(`/api/addVictim?victimID=${encodeURIComponent(this.addVictimID)}&caseID=${encodeURIComponent(this.addCaseID)}&caseType=${encodeURIComponent(this.addCaseType)}`);"string"==typeof e.data?(0,I.z8)({showClose:!0,message:e.data,type:"warning",duration:5e3}):(await this.fetchVictimInfo(),(0,I.z8)({showClose:!0,message:"新增受害人成功! ",type:"success",duration:5e3}))}catch(e){(0,I.z8)({showClose:!0,message:"新增受害人失败！",type:"error",duration:5e3})}},async delVictimInfo(){try{const e=await C.Z.delete(`/api/delVictim?victimID=${encodeURIComponent(this.delVictimID)}&caseID=${encodeURIComponent(this.delCaseID)}&caseType=${encodeURIComponent(this.delCaseType)}`);"string"==typeof e.data?(0,I.z8)({showClose:!0,message:e.data,type:"warning",duration:5e3}):(await this.fetchVictimInfo(),(0,I.z8)({showClose:!0,message:"删除受害人成功! ",type:"success",duration:5e3}))}catch(e){(0,I.z8)({showClose:!0,message:"删除受害人失败！",type:"error",duration:5e3})}},async back(){this.caseInfo="",this.showCase=!1},typeToID(e){return""===e?"":"强奸"===e?"A":"抢劫"===e?"R":"故意伤害"===e?"H":"盗窃"===e?"T":"诈骗"===e?"C":"谋杀"===e?"M":((0,I.z8)({showClose:!0,message:"案件类型转换错误！",type:"error",duration:5e3}),"")}},watch:{addCaseType(){this.type1=this.typeToID(this.addCaseType)},delCaseType(){this.type2=this.typeToID(this.delCaseType)}},async created(){await this.fetchVictimInfo()}},W=t(89);const V=(0,W.Z)(_,[["render",b],["__scopeId","data-v-4d10e96b"]]);var k=V}}]);
//# sourceMappingURL=530.c239fa1d.js.map