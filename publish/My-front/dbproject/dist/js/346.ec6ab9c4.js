"use strict";(self["webpackChunkdbproject"]=self["webpackChunkdbproject"]||[]).push([[346],{5346:function(e,a,t){t.r(a),t.d(a,{default:function(){return U}});var l=t(3396),s=t(9242),d=t(7139);const o=e=>((0,l.dD)("data-v-28694a5a"),e=e(),(0,l.Cn)(),e),p=o((()=>(0,l._)("div",null,"  案件管理 > 嫌疑人信息管理",-1))),n={style:{"text-align":"center"}},u={style:{display:"inline-block","text-align":"left"}},r={class:"seaButton"},i={style:{"text-align":"center"}},c={style:{display:"inline-block","text-align":"left"}},m={class:"qianru"},h=o((()=>(0,l._)("td",null,null,-1))),y={class:"seaButton"},w={style:{"text-align":"center"}},f={style:{display:"inline-block","text-align":"left"}},g={class:"qianru"},x=o((()=>(0,l._)("td",null,null,-1))),b={class:"seaButton"};function C(e,a,t,o,C,I){const _=(0,l.up)("el-header"),W=(0,l.up)("el-text"),v=(0,l.up)("el-input"),k=(0,l.up)("el-option"),U=(0,l.up)("el-select"),T=(0,l.up)("el-button"),V=(0,l.up)("el-divider"),D=(0,l.up)("el-table-column"),S=(0,l.up)("el-table"),$=(0,l.up)("el-tab-pane"),z=(0,l.up)("el-tabs");return(0,l.wg)(),(0,l.iD)(l.HY,null,[(0,l.Wm)(_,{class:"sub-header",onMousemove:I.handleMouseMove},{default:(0,l.w5)((()=>[p])),_:1},8,["onMousemove"]),(0,l.Wm)(z,{modelValue:C.anv,"onUpdate:modelValue":a[12]||(a[12]=e=>C.anv=e),type:"border-card",style:{"margin-top":"10vh"}},{default:(0,l.w5)((()=>[(0,l.Wm)($,{label:"查询嫌疑人",name:"1"},{default:(0,l.w5)((()=>[(0,l._)("div",n,[(0,l._)("table",u,[(0,l._)("tr",null,[(0,l._)("td",null,[(0,l.Wm)(W,{class:"noteText",type:"primary"},{default:(0,l.w5)((()=>[(0,l.Uk)("姓名：")])),_:1}),(0,l.Wm)(v,{class:"inputBox",modelValue:C.suspectName,"onUpdate:modelValue":a[0]||(a[0]=e=>C.suspectName=e),placeholder:"请输入姓名",clearable:""},null,8,["modelValue"])]),(0,l._)("td",null,[(0,l.Wm)(W,{class:"noteText",type:"primary"},{default:(0,l.w5)((()=>[(0,l.Uk)("性别：")])),_:1}),(0,l.Wm)(U,{class:"inputBox",modelValue:C.suspectSex,"onUpdate:modelValue":a[1]||(a[1]=e=>C.suspectSex=e),placeholder:"请选择",style:{width:"235px"}},{default:(0,l.w5)((()=>[(0,l.Wm)(k,{selected:"",label:"全部性别",value:""}),(0,l.Wm)(k,{label:"男",value:"M"}),(0,l.Wm)(k,{label:"女",value:"F"})])),_:1},8,["modelValue"])])]),(0,l._)("tr",null,[(0,l._)("td",null,[(0,l.Wm)(W,{class:"noteText",type:"primary"},{default:(0,l.w5)((()=>[(0,l.Uk)("住址：")])),_:1}),(0,l.Wm)(v,{class:"inputBox",modelValue:C.suspectAddress,"onUpdate:modelValue":a[2]||(a[2]=e=>C.suspectAddress=e),placeholder:"请输入住址",clearable:""},null,8,["modelValue"])]),(0,l._)("td",null,[(0,l.Wm)(W,{class:"noteText",type:"primary"},{default:(0,l.w5)((()=>[(0,l.Uk)("身份证号：")])),_:1}),(0,l.Wm)(v,{class:"inputBox",modelValue:C.suspectID,"onUpdate:modelValue":a[3]||(a[3]=e=>C.suspectID=e),placeholder:"请输入身份证号码",oninput:"value=value.replace(/[^\\d.]/g,'')",clearable:"",maxlength:"18",style:{width:"235px"},"show-word-limit":""},null,8,["modelValue"])])])]),(0,l._)("div",r,[(0,l.Wm)(T,{type:"primary",onClick:I.fetchSuspectInfo},{default:(0,l.w5)((()=>[(0,l.Uk)("查询")])),_:1},8,["onClick"])])]),(0,l.Wm)(V),(0,l._)("div",null,[C.showCase?(0,l.kq)("",!0):((0,l.wg)(),(0,l.j4)(S,{key:0,data:C.chanSuspectInfo,stripe:"",height:"450",onWheelPassive:a[4]||(a[4]=(0,s.iM)((()=>{}),["stop"]))},{default:(0,l.w5)((()=>[(0,l.Wm)(D,{prop:"id",label:"身份证号",sortable:"",width:"250px"}),(0,l.Wm)(D,{prop:"name",label:"姓名",sortable:"",width:"150px"}),(0,l.Wm)(D,{prop:"sex",label:"性别",sortable:"",width:"150px"}),(0,l.Wm)(D,{prop:"address",label:"住址",sortable:""}),(0,l.Wm)(D,{align:"right",width:"100px"},{header:(0,l.w5)((()=>[])),default:(0,l.w5)((e=>[(0,l.Wm)(T,{size:"small",type:"primary",onClick:a=>I.getCase(e.$index,e.row)},{default:(0,l.w5)((()=>[(0,l.Uk)("涉及案件")])),_:2},1032,["onClick"])])),_:1})])),_:1},8,["data"])),C.showCase&&C.caseInfo.length>0?((0,l.wg)(),(0,l.j4)(S,{key:1,data:C.caseInfo,stripe:"",height:"450",onWheelPassive:a[5]||(a[5]=(0,s.iM)((()=>{}),["stop"]))},{default:(0,l.w5)((()=>[(0,l.Wm)(D,{prop:"id",label:"案件编号",sortable:"",width:"120px"}),(0,l.Wm)(D,{prop:"type",label:"案件类型",sortable:"",width:"120px"}),(0,l.Wm)(D,{prop:"status",label:"案件状态",sortable:"",width:"120px"}),(0,l.Wm)(D,{prop:"registerTime",label:"登记时间",sortable:""}),(0,l.Wm)(D,{prop:"address",label:"案发地点",sortable:""}),(0,l.Wm)(D,{prop:"ranking",label:"案件等级",sortable:"",width:"120px"}),(0,l.Wm)(D,{align:"center",width:"80px"},{header:(0,l.w5)((()=>[(0,l.Wm)(T,{size:"small",type:"primary",onClick:I.back},{default:(0,l.w5)((()=>[(0,l.Uk)("返回")])),_:1},8,["onClick"])])),_:1})])),_:1},8,["data"])):(0,l.kq)("",!0)])])),_:1}),(0,l.Wm)($,{label:"新增嫌疑人",name:"2"},{default:(0,l.w5)((()=>[(0,l._)("div",i,[(0,l._)("table",c,[(0,l._)("tr",null,[(0,l._)("td",null,[(0,l.Wm)(W,{class:"noteText",type:"primary",style:{"margin-right":"10px"}},{default:(0,l.w5)((()=>[(0,l.Uk)("案件编号:")])),_:1}),(0,l.Wm)(v,{modelValue:C.addCaseID,"onUpdate:modelValue":a[6]||(a[6]=e=>C.addCaseID=e),placeholder:"案件编号",clearable:"",maxlength:"7",oninput:"value=value.replace(/[^\\d.]/g,'')","show-word-limit":"",style:{width:"190px"}},{prepend:(0,l.w5)((()=>[(0,l._)("div",m,(0,d.zw)(C.type1),1)])),_:1},8,["modelValue"])]),(0,l._)("td",null,[(0,l.Wm)(W,{class:"noteText",type:"primary"},{default:(0,l.w5)((()=>[(0,l.Uk)("案件类型:")])),_:1}),(0,l.Wm)(U,{class:"inputBox",modelValue:C.addCaseType,"onUpdate:modelValue":a[7]||(a[7]=e=>C.addCaseType=e),placeholder:"请选择",style:{width:"235px"}},{default:(0,l.w5)((()=>[(0,l.Wm)(k,{label:"强奸",value:"强奸"}),(0,l.Wm)(k,{label:"抢劫",value:"抢劫"}),(0,l.Wm)(k,{label:"故意伤害",value:"故意伤害"}),(0,l.Wm)(k,{label:"盗窃",value:"盗窃"}),(0,l.Wm)(k,{label:"诈骗",value:"诈骗"}),(0,l.Wm)(k,{label:"谋杀",value:"谋杀"})])),_:1},8,["modelValue"])])]),(0,l._)("tr",null,[h,(0,l._)("td",null,[(0,l.Wm)(W,{class:"noteText",type:"primary",style:{"margin-left":"7px"}},{default:(0,l.w5)((()=>[(0,l.Uk)("身份证号：")])),_:1}),(0,l.Wm)(v,{class:"inputBox",modelValue:C.addSuspectID,"onUpdate:modelValue":a[8]||(a[8]=e=>C.addSuspectID=e),placeholder:"请输入身份证号",oninput:"value=value.replace(/[^\\d.]/g,'')",style:{"margin-left":"3px",width:"235px"},clearable:"",maxlength:"18","show-word-limit":""},null,8,["modelValue"])])])]),(0,l._)("div",y,[(0,l.Wm)(T,{type:"primary",onClick:I.addSuspectInfo},{default:(0,l.w5)((()=>[(0,l.Uk)("新增嫌疑人")])),_:1},8,["onClick"])])])])),_:1}),(0,l.Wm)($,{label:"删除嫌疑人",name:"3"},{default:(0,l.w5)((()=>[(0,l._)("div",w,[(0,l._)("table",f,[(0,l._)("tr",null,[(0,l._)("td",null,[(0,l.Wm)(W,{class:"noteText",type:"primary",style:{"margin-right":"10px"}},{default:(0,l.w5)((()=>[(0,l.Uk)("案件编号:")])),_:1}),(0,l.Wm)(v,{modelValue:C.delCaseID,"onUpdate:modelValue":a[9]||(a[9]=e=>C.delCaseID=e),placeholder:"案件编号",clearable:"",maxlength:"7",oninput:"value=value.replace(/[^\\d.]/g,'')","show-word-limit":"",style:{width:"190px"}},{prepend:(0,l.w5)((()=>[(0,l._)("div",g,(0,d.zw)(C.type2),1)])),_:1},8,["modelValue"])]),(0,l._)("td",null,[(0,l.Wm)(W,{class:"noteText",type:"primary"},{default:(0,l.w5)((()=>[(0,l.Uk)("案件类型:")])),_:1}),(0,l.Wm)(U,{class:"inputBox",modelValue:C.delCaseType,"onUpdate:modelValue":a[10]||(a[10]=e=>C.delCaseType=e),placeholder:"请选择",style:{width:"235px"}},{default:(0,l.w5)((()=>[(0,l.Wm)(k,{label:"强奸",value:"强奸"}),(0,l.Wm)(k,{label:"抢劫",value:"抢劫"}),(0,l.Wm)(k,{label:"故意伤害",value:"故意伤害"}),(0,l.Wm)(k,{label:"盗窃",value:"盗窃"}),(0,l.Wm)(k,{label:"诈骗",value:"诈骗"}),(0,l.Wm)(k,{label:"谋杀",value:"谋杀"})])),_:1},8,["modelValue"])])]),(0,l._)("tr",null,[x,(0,l._)("td",null,[(0,l.Wm)(W,{class:"noteText",type:"primary",style:{"margin-left":"7px"}},{default:(0,l.w5)((()=>[(0,l.Uk)("身份证号：")])),_:1}),(0,l.Wm)(v,{class:"inputBox",modelValue:C.delSuspectID,"onUpdate:modelValue":a[11]||(a[11]=e=>C.delSuspectID=e),placeholder:"请输入身份证号",oninput:"value=value.replace(/[^\\d.]/g,'')",style:{"margin-left":"3px",width:"235px"},clearable:"",maxlength:"18","show-word-limit":""},null,8,["modelValue"])])])]),(0,l._)("div",b,[(0,l.Wm)(T,{type:"primary",onClick:I.delSuspectInfo},{default:(0,l.w5)((()=>[(0,l.Uk)("删除嫌疑人")])),_:1},8,["onClick"])])])])),_:1})])),_:1},8,["modelValue"])],64)}var I=t(7138),_=t(7178),W={data(){return{anv:"1",showCase:!1,suspectID:"",suspectName:"",suspectSex:"",suspectAddress:"",suspectInfo:[],chanSuspectInfo:[],caseInfo:[],addSuspectID:"",addCaseID:"",addCaseType:"",type1:"",delSuspectID:"",delCaseID:"",delCaseType:"",type2:""}},methods:{handleMouseMove(e){const a=e.pageX-e.target.offsetLeft,t=e.pageY-e.target.offsetTop;e.target.style.setProperty("--x",`${a}px`),e.target.style.setProperty("--y",`${t}px`)},async fetchSuspectInfo(){try{const e=await I.Z.get(`/api/suspectInfo?suspectID=${encodeURIComponent(this.suspectID)}&suspectName=${encodeURIComponent(this.suspectName)}&suspectSex=${encodeURIComponent(this.suspectSex)}&suspectAddress=${encodeURIComponent(this.suspectAddress)}`);this.suspectInfo=e.data,this.chanSuspectInfo=this.suspectInfo.map((e=>"M"===e.sex?{...e,sex:"男"}:"F"===e.sex?{...e,sex:"女"}:e))}catch(e){(0,_.z8)({showClose:!0,message:"获取嫌疑人列表失败！",type:"error",duration:5e3})}},async getCase(e,a){try{const e=await I.Z.get(`/api/getCaseS?suspectID=${encodeURIComponent(a.id)}`);for(let a=0;a<e.data.length;a++)e.data[a].registerTime=e.data[a].registerTime.replace("T"," "),"0"==e.data[a].ranking?e.data[a].ranking="一般":"1"==e.data[a].ranking?e.data[a].ranking="较大":"2"==e.data[a].ranking?e.data[a].ranking="重大":"3"==e.data[a].ranking&&(e.data[a].ranking="特别重大");this.caseInfo=e.data,this.showCase=!0}catch(t){(0,_.z8)({showClose:!0,message:"获取案件信息失败！",type:"error",duration:5e3})}},async addSuspectInfo(){try{const e=await I.Z.post(`/api/addSuspect?suspectID=${encodeURIComponent(this.addSuspectID)}&caseID=${encodeURIComponent(this.addCaseID)}&caseType=${encodeURIComponent(this.addCaseType)}`);"string"==typeof e.data?(0,_.z8)({showClose:!0,message:e.data,type:"warning",duration:5e3}):(await this.fetchSuspectInfo(),(0,_.z8)({showClose:!0,message:"新增嫌疑人成功! ",type:"success",duration:5e3}))}catch(e){(0,_.z8)({showClose:!0,message:"新增嫌疑人失败！",type:"error",duration:5e3})}},async delSuspectInfo(){try{const e=await I.Z.delete(`/api/delSuspect?suspectID=${encodeURIComponent(this.delSuspectID)}&caseID=${encodeURIComponent(this.delCaseID)}&caseType=${encodeURIComponent(this.delCaseType)}`);"string"==typeof e.data?(0,_.z8)({showClose:!0,message:e.data,type:"warning",duration:5e3}):(await this.fetchSuspectInfo(),(0,_.z8)({showClose:!0,message:"删除嫌疑人成功! ",type:"success",duration:5e3}))}catch(e){(0,_.z8)({showClose:!0,message:"删除嫌疑人失败！",type:"error",duration:5e3})}},async back(){this.caseInfo="",this.showCase=!1},typeToID(e){return""===e?"":"强奸"===e?"A":"抢劫"===e?"R":"故意伤害"===e?"H":"盗窃"===e?"T":"诈骗"===e?"C":"谋杀"===e?"M":((0,_.z8)({showClose:!0,message:"案件类型转换错误！",type:"error",duration:5e3}),"")}},watch:{addCaseType(){this.type1=this.typeToID(this.addCaseType)},delCaseType(){this.type2=this.typeToID(this.delCaseType)}},async created(){await this.fetchSuspectInfo()}},v=t(89);const k=(0,v.Z)(W,[["render",C],["__scopeId","data-v-28694a5a"]]);var U=k}}]);
//# sourceMappingURL=346.ec6ab9c4.js.map