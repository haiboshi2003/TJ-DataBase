"use strict";(self["webpackChunkdbproject"]=self["webpackChunkdbproject"]||[]).push([[410],{410:function(e,t,l){l.r(t),l.d(t,{default:function(){return V}});var s=l(3396),n=l(9242),u=l(7139);const a=e=>((0,s.dD)("data-v-030a8e6c"),e=e(),(0,s.Cn)(),e),_=a((()=>(0,s._)("div",null,"  警员管理 > 修改申请管理",-1))),o={class:"container"},c=a((()=>(0,s._)("div",{class:"ssqtitletest"},[(0,s._)("span",null,"权 限 申 请 列 表")],-1))),r={class:"custom-table"},h={style:{width:"20px"}},d=a((()=>(0,s._)("th",{style:{width:"80px"}},"申请人",-1))),p=a((()=>(0,s._)("th",{style:{width:"100px"}},"被修改人",-1))),i=a((()=>(0,s._)("th",{style:{width:"100px"}},"原权限等级",-1))),b=a((()=>(0,s._)("th",{style:{width:"120px"}},"修改权限等级",-1))),v=a((()=>(0,s._)("th",{style:{width:"80px"}},"申请状态",-1))),w=a((()=>(0,s._)("th",{style:{width:"200px"}},"申请原因",-1))),m=a((()=>(0,s._)("th",{style:{width:"80px"}},"操作",-1))),g=["onUpdate:modelValue"],y={class:"wrap-cell"},f=["onClick"],q=a((()=>(0,s._)("span",null,"同意",-1))),R=[q],k=["onClick"],z=a((()=>(0,s._)("span",null,"拒绝",-1))),x=[z],j=a((()=>(0,s._)("span",null,"批量同意",-1))),D=[j],A=a((()=>(0,s._)("span",null,"批量拒绝",-1))),C=[A],M=a((()=>(0,s._)("div",{class:"ssqtitletest",style:{"margin-top":"80px"}},[(0,s._)("span",null,"已 同 意 的 申 请")],-1))),L={class:"custom-table"},F=a((()=>(0,s._)("thead",null,[(0,s._)("tr",null,[(0,s._)("th",null,"申请人警号"),(0,s._)("th",null,"被修改人警号"),(0,s._)("th",null,"原权限等级"),(0,s._)("th",null,"修改权限等级"),(0,s._)("th",null,"申请状态"),(0,s._)("th",null,"申请原因")])],-1))),Y=a((()=>(0,s._)("div",{class:"ssqtitletest",style:{"margin-top":"50px"}},[(0,s._)("span",null,"已 拒 绝 的 申 请")],-1))),E={class:"custom-table"},H=a((()=>(0,s._)("thead",null,[(0,s._)("tr",null,[(0,s._)("th",null,"申请人警号"),(0,s._)("th",null,"被修改人警号"),(0,s._)("th",null,"原权限等级"),(0,s._)("th",null,"修改权限等级"),(0,s._)("th",null,"申请状态"),(0,s._)("th",null,"申请原因")])],-1)));function Z(e,t,l,a,q,z){const j=(0,s.up)("el-header");return(0,s.wg)(),(0,s.iD)(s.HY,null,[(0,s.Wm)(j,{class:"sub-header",onMousemove:z.handleMouseMove},{default:(0,s.w5)((()=>[_])),_:1},8,["onMousemove"]),(0,s._)("div",o,[(0,s._)("div",{class:"content",onWheelPassive:t[4]||(t[4]=(0,n.iM)((()=>{}),["stop"]))},[c,(0,s._)("table",r,[(0,s._)("thead",null,[(0,s._)("tr",null,[(0,s._)("th",h,[(0,s.wy)((0,s._)("input",{type:"checkbox","onUpdate:modelValue":t[0]||(t[0]=e=>q.selectAll=e),onChange:t[1]||(t[1]=(...e)=>z.toggleSelectAll&&z.toggleSelectAll(...e))},null,544),[[n.e8,q.selectAll]])]),d,p,i,b,v,w,m])]),(0,s._)("tbody",null,[((0,s.wg)(!0),(0,s.iD)(s.HY,null,(0,s.Ko)(q.pendingRequests,(e=>((0,s.wg)(),(0,s.iD)("tr",{key:e.h_number+"-"+e.s_number+"-"+e.F_level+"-"+e.L_level},[(0,s._)("td",null,["待处理"===e.status?(0,s.wy)(((0,s.wg)(),(0,s.iD)("input",{key:0,type:"checkbox","onUpdate:modelValue":t=>e.selected=t},null,8,g)),[[n.e8,e.selected]]):(0,s.kq)("",!0)]),(0,s._)("td",null,(0,u.zw)(e.h_number),1),(0,s._)("td",null,(0,u.zw)(e.s_number),1),(0,s._)("td",null,(0,u.zw)(e.f_level),1),(0,s._)("td",null,(0,u.zw)(e.l_level),1),(0,s._)("td",null,(0,u.zw)(e.status),1),(0,s._)("td",y,(0,u.zw)(e.reason),1),(0,s._)("td",null,["待处理"===e.status?((0,s.wg)(),(0,s.iD)("button",{key:0,class:"cpbutton",style:{"margin-top":"-5px"},onClick:t=>z.approveRequest(e)},R,8,f)):(0,s.kq)("",!0),"待处理"===e.status?((0,s.wg)(),(0,s.iD)("button",{key:1,class:"cpbutton",onClick:t=>z.rejectRequest(e)},x,8,k)):(0,s.kq)("",!0)])])))),128))])]),(0,s._)("div",null,[(0,s._)("button",{class:"cpbutton1",onClick:t[2]||(t[2]=(...e)=>z.batchApprove&&z.batchApprove(...e))},D),(0,s._)("button",{class:"cpbutton1",style:{"margin-left":"40px"},onClick:t[3]||(t[3]=(...e)=>z.batchReject&&z.batchReject(...e))},C)]),M,(0,s._)("table",L,[F,(0,s._)("tbody",null,[((0,s.wg)(!0),(0,s.iD)(s.HY,null,(0,s.Ko)(q.approvedRequests,(e=>((0,s.wg)(),(0,s.iD)("tr",{key:e.h_number+"-"+e.s_number+"-"+e.F_level+"-"+e.L_level},[(0,s._)("td",null,(0,u.zw)(e.h_number),1),(0,s._)("td",null,(0,u.zw)(e.s_number),1),(0,s._)("td",null,(0,u.zw)(e.f_level),1),(0,s._)("td",null,(0,u.zw)(e.l_level),1),(0,s._)("td",null,(0,u.zw)(e.status),1),(0,s._)("td",null,(0,u.zw)(e.reason),1)])))),128))])]),Y,(0,s._)("table",E,[H,(0,s._)("tbody",null,[((0,s.wg)(!0),(0,s.iD)(s.HY,null,(0,s.Ko)(q.rejectedRequests,(e=>((0,s.wg)(),(0,s.iD)("tr",{key:e.h_number+"-"+e.s_number+"-"+e.F_level+"-"+e.L_level},[(0,s._)("td",null,(0,u.zw)(e.h_number),1),(0,s._)("td",null,(0,u.zw)(e.s_number),1),(0,s._)("td",null,(0,u.zw)(e.f_level),1),(0,s._)("td",null,(0,u.zw)(e.l_level),1),(0,s._)("td",null,(0,u.zw)(e.status),1),(0,s._)("td",null,(0,u.zw)(e.reason),1)])))),128))])])],32)])],64)}l(7658);var K=l(7138),P={data(){return{pendingRequests:[],approvedRequests:[],rejectedRequests:[],selectAll:!1}},mounted(){this.fetchRequests()},methods:{handleMouseMove(e){const t=e.pageX-e.target.offsetLeft,l=e.pageY-e.target.offsetTop;e.target.style.setProperty("--x",`${t}px`),e.target.style.setProperty("--y",`${l}px`)},toggleSelectAll(){this.pendingRequests.forEach((e=>{e.selected=this.selectAll}))},fetchRequests(){K.Z.get("/api/manage").then((e=>{const t=[],l=[],s=[];e.data.forEach((e=>{"待处理"===e.status?t.push(e):"同意"===e.status?l.push(e):"拒绝"===e.status&&s.push(e)})),this.pendingRequests=t,this.approvedRequests=l,this.rejectedRequests=s})).catch((e=>{console.error(e)}))},approveRequest(e){K.Z.post("/api/manage",{s_number:e.s_number,h_number:e.h_number,F_level:e.f_level,L_level:e.l_level,reason:e.reason,status:"同意"}).then((e=>{this.fetchRequests()})).catch((e=>{console.error(e)}))},rejectRequest(e){K.Z.post("/api/manage",{s_number:e.s_number,h_number:e.h_number,F_level:e.f_level,L_level:e.l_level,reason:e.reason,status:"拒绝"}).then((e=>{this.fetchRequests()})).catch((e=>{console.error(e)}))},batchApprove(){const e=this.pendingRequests.filter((e=>e.selected));e.forEach((e=>{this.approveRequest(e)}))},batchReject(){const e=this.pendingRequests.filter((e=>e.selected));e.forEach((e=>{this.rejectRequest(e)}))}}},S=l(89);const U=(0,S.Z)(P,[["render",Z],["__scopeId","data-v-030a8e6c"]]);var V=U}}]);
//# sourceMappingURL=410.0674d1ae.js.map