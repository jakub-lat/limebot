(function(t){function e(e){for(var n,s,o=e[0],c=e[1],l=e[2],v=0,d=[];v<o.length;v++)s=o[v],Object.prototype.hasOwnProperty.call(r,s)&&r[s]&&d.push(r[s][0]),r[s]=0;for(n in c)Object.prototype.hasOwnProperty.call(c,n)&&(t[n]=c[n]);u&&u(e);while(d.length)d.shift()();return i.push.apply(i,l||[]),a()}function a(){for(var t,e=0;e<i.length;e++){for(var a=i[e],n=!0,o=1;o<a.length;o++){var c=a[o];0!==r[c]&&(n=!1)}n&&(i.splice(e--,1),t=s(s.s=a[0]))}return t}var n={},r={app:0},i=[];function s(e){if(n[e])return n[e].exports;var a=n[e]={i:e,l:!1,exports:{}};return t[e].call(a.exports,a,a.exports,s),a.l=!0,a.exports}s.m=t,s.c=n,s.d=function(t,e,a){s.o(t,e)||Object.defineProperty(t,e,{enumerable:!0,get:a})},s.r=function(t){"undefined"!==typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})},s.t=function(t,e){if(1&e&&(t=s(t)),8&e)return t;if(4&e&&"object"===typeof t&&t&&t.__esModule)return t;var a=Object.create(null);if(s.r(a),Object.defineProperty(a,"default",{enumerable:!0,value:t}),2&e&&"string"!=typeof t)for(var n in t)s.d(a,n,function(e){return t[e]}.bind(null,n));return a},s.n=function(t){var e=t&&t.__esModule?function(){return t["default"]}:function(){return t};return s.d(e,"a",e),e},s.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},s.p="/";var o=window["webpackJsonp"]=window["webpackJsonp"]||[],c=o.push.bind(o);o.push=e,o=o.slice();for(var l=0;l<o.length;l++)e(o[l]);var u=c;i.push([0,"chunk-vendors"]),a()})({0:function(t,e,a){t.exports=a("56d7")},"56d7":function(t,e,a){"use strict";a.r(e);a("e260"),a("e6cf"),a("cca6"),a("a79d");var n=a("2b0e"),r=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("router-view")},i=[],s={name:"App"},o=s,c=a("2877"),l=Object(c["a"])(o,r,i,!1,null,null,null),u=l.exports,v=a("8c4f"),d=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("v-app",[a("v-navigation-drawer",{staticClass:"d-sm-none",attrs:{app:"",temporary:""},model:{value:t.drawer,callback:function(e){t.drawer=e},expression:"drawer"}},[a("v-list-item",[a("v-list-item-content",[a("v-list-item-title",{staticClass:"title"},[t._v(" Lime ")]),a("v-list-item-subtitle",[t._v(" A multi-purpose Discord Bot ")])],1)],1),a("v-divider"),a("v-list",{attrs:{dense:""}},[a("v-list-item",{attrs:{link:"",to:"/"}},[a("v-list-item-action",[a("v-icon",[t._v("mdi-home")])],1),a("v-list-item-content",[a("v-list-item-title",[t._v("Start")])],1)],1),a("v-list-item",{attrs:{link:"",to:"/#features"}},[a("v-list-item-action",[a("v-icon",[t._v("mdi-palette")])],1),a("v-list-item-content",[a("v-list-item-title",[t._v("Features")])],1)],1),a("v-list-item",{attrs:{link:"",to:"/commands"}},[a("v-list-item-action",[a("v-icon",[t._v("mdi-view-list")])],1),a("v-list-item-content",[a("v-list-item-title",[t._v("Commands")])],1)],1)],1)],1),a("v-app-bar",{attrs:{app:"","clipped-left":"",color:"primary"}},[a("v-app-bar-nav-icon",{staticClass:"d-sm-none",on:{click:function(e){e.stopPropagation(),t.drawer=!t.drawer}}}),a("div",{staticClass:"ml-5 d-none d-sm-block"},[a("v-btn",{attrs:{text:"",to:"/"}},[a("v-icon",[t._v("mdi-home")]),a("span",{attrs:{left:""}},[t._v("Start")])],1),a("v-btn",{attrs:{text:"",to:"/#features"}},[a("v-icon",{attrs:{left:""}},[t._v("mdi-palette")]),a("span",[t._v("Features")])],1),a("v-btn",{attrs:{text:"",to:"/commands"}},[a("v-icon",{attrs:{left:""}},[t._v("mdi-view-list")]),a("span",[t._v("Commands")])],1)],1),a("v-spacer"),a("v-btn",{attrs:{to:"/manage",text:""}},[a("span",{staticClass:"mr-2"},[t._v("Dashboard")]),a("v-icon",[t._v("mdi-login-variant")])],1)],1),a("v-content",[a("router-view")],1)],1)},m=[],p={data:function(){return{drawer:null}}},f=p,g=a("6544"),b=a.n(g),h=a("7496"),_=a("40dc"),x=a("5bc1"),w=a("8336"),V=a("a75b"),C=a("ce7e"),y=a("132d"),k=a("8860"),j=a("da13"),S=a("1800"),O=a("5d23"),$=a("f774"),I=a("2fa4"),L=Object(c["a"])(f,d,m,!1,null,null,null),E=L.exports;b()(L,{VApp:h["a"],VAppBar:_["a"],VAppBarNavIcon:x["a"],VBtn:w["a"],VContent:V["a"],VDivider:C["a"],VIcon:y["a"],VList:k["a"],VListItem:j["a"],VListItemAction:S["a"],VListItemContent:O["a"],VListItemSubtitle:O["b"],VListItemTitle:O["c"],VNavigationDrawer:$["a"],VSpacer:I["a"]});var A=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("v-app",[!1!==this.$route.meta.nav&&t.loaded?a("v-navigation-drawer",{attrs:{app:"",clipped:""},scopedSlots:t._u([{key:"append",fn:function(){return[a("v-list-item",{staticClass:"mb-3",attrs:{exact:"",link:"",to:"/manage"}},[a("v-list-item-action",[a("v-icon",[t._v("mdi-view-list")])],1),a("v-list-item-content",[a("v-list-item-title",[t._v("Switch server")])],1)],1)]},proxy:!0}],null,!1,724294604),model:{value:t.drawer,callback:function(e){t.drawer=e},expression:"drawer"}},[a("v-list-item",{staticClass:"px-2 my-2"},[a("v-list-item-avatar",[a("v-img",{attrs:{src:t.serverIcon}})],1),a("v-list-item-content",[a("v-list-item-title",{staticClass:"title"},[t._v(" "+t._s(t.server.name)+" ")]),a("v-list-item-subtitle",[t._v(" Manage server ")])],1)],1),a("v-divider"),a("v-list",{attrs:{dense:""}},[a("v-list-item",{attrs:{exact:"",link:"",to:"/manage/"+this.$route.params.id+"/"}},[a("v-list-item-action",[a("v-icon",[t._v("mdi-view-dashboard")])],1),a("v-list-item-content",[a("v-list-item-title",[t._v("Dashboard")])],1)],1),a("v-list-item",{attrs:{exact:"",link:"",to:"/manage/"+this.$route.params.id+"/general"}},[a("v-list-item-action",[a("v-icon",[t._v("mdi-cogs")])],1),a("v-list-item-content",[a("v-list-item-title",[t._v("General settings")])],1)],1),a("v-divider"),a("v-list-item",{attrs:{exact:"",link:"",to:"/commands"}},[a("v-list-item-action",[a("v-icon",[t._v("mdi-view-list")])],1),a("v-list-item-content",[a("v-list-item-title",[t._v("Commands")])],1)],1)],1)],1):t._e(),a("v-app-bar",{attrs:{app:"","clipped-left":"",color:"primary"}},[!1!==this.$route.meta.nav?a("v-app-bar-nav-icon",{on:{click:function(e){e.stopPropagation(),t.drawer=!t.drawer}}}):t._e(),a("v-toolbar-title",{staticClass:"d-none d-sm-block"},[t._v("Lime Dashboard")]),a("v-spacer"),a("v-menu",{attrs:{"offset-y":""},scopedSlots:t._u([{key:"activator",fn:function(e){var n=e.on;return[a("v-btn",t._g({staticClass:"mr-1",attrs:{icon:"",loading:!t.loaded}},n),[a("v-avatar",[a("v-img",{attrs:{src:"https://cdn.discordapp.com/avatars/"+t.user.id+"/"+t.user.avatar+".png?size=128"}})],1)],1)]}}])},[a("v-list",[a("v-list-item",[a("v-list-item-title",[a("b",[t._v(t._s(t.user.username)+"#"+t._s(t.user.discriminator))])])],1),a("v-list-item",{attrs:{to:"/"}},[a("v-list-item-title",[t._v("Back to website")]),a("v-list-item-icon",[a("v-icon",[t._v("mdi-web")])],1)],1),a("v-list-item",{attrs:{to:"/logout"}},[a("v-list-item-title",[t._v("Logout")]),a("v-list-item-icon",[a("v-icon",[t._v("mdi-login-variant")])],1)],1)],1)],1)],1),a("v-content",[a("v-container",{attrs:{fluid:""}},[t.loaded?a("router-view"):a("v-progress-linear",{attrs:{indeterminate:"",color:"primary"}})],1)],1),a("v-snackbar",{attrs:{color:"error",timeout:t.timeout},model:{value:t.unsaved,callback:function(e){t.unsaved=e},expression:"unsaved"}},[t._v(" You have unsaved changes! "),a("v-btn",{attrs:{color:"primary",loading:t.saveLoading,disabled:t.saveLoading},on:{click:t.save}},[t._v(" Save ")])],1)],1)},P=[],R=(a("99af"),a("7db0"),a("5530")),T=(a("96cf"),a("1da1")),B=a("2f62");n["default"].use(B["a"]);var D=new B["a"].Store({state:{settings:{},server:{},unsaved:!1,user:{},token:null},mutations:{set:function(t,e){t.settings=Object(R["a"])({},D.settings,{},e),t.unsaved=!0},setSettings:function(t,e){t.settings=e,t.unsaved=!1},setServer:function(t,e){t.server=e},unsaved:function(t,e){t.unsaved=!!e},setUser:function(t,e){t.user=e}}}),M=D,G={data:function(){return{drawer:null,timeout:0,loaded:!1,saveLoading:!1}},methods:{loadServer:function(){var t=this;return Object(T["a"])(regeneratorRuntime.mark((function e(){var a,n;return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:if(a=t.$route.params.id,a){e.next=6;break}return M.commit("unsaved",!1),M.commit("setServer",{}),M.commit("setSettings",{}),e.abrupt("return");case 6:if(t.server.id!=a){e.next=8;break}return e.abrupt("return");case 8:return t.loaded=!1,e.next=11,t.$api.get("guild/".concat(a));case 11:n=e.sent,t.loaded=!0,M.commit("setSettings",Object(R["a"])({},n,{id:a})),M.commit("setServer",Object(R["a"])({},t.user.guilds.find((function(t){return t.id==a}))));case 15:case"end":return e.stop()}}),e)})))()},save:function(){var t=this;return Object(T["a"])(regeneratorRuntime.mark((function e(){var a,n;return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:return e.prev=0,t.saveLoading=!0,a=Object(R["a"])({},M.state.settings,{id:t.$route.params.id}),e.next=5,t.$api.put("guild/".concat(t.$route.params.id),a);case 5:n=e.sent,n&&(M.commit("unsaved",!1),t.saveLoading=!1),e.next=12;break;case 9:e.prev=9,e.t0=e["catch"](0),console.log(e.t0);case 12:case"end":return e.stop()}}),e,null,[[0,9]])})))()}},watch:{$route:"loadServer"},created:function(){var t=this;return Object(T["a"])(regeneratorRuntime.mark((function e(){var a,n;return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:if(a=t.$cookies.get("token"),a){e.next=5;break}window.location.href=t.$api.path("auth"),e.next=18;break;case 5:return e.prev=5,e.next=8,t.$api.get("auth/user");case 8:if(n=e.sent,n.id){e.next=11;break}throw"wrong user response";case 11:M.commit("setUser",n),t.loaded=!0,e.next=18;break;case 15:e.prev=15,e.t0=e["catch"](5),window.location.href=t.$api.path("auth");case 18:t.loadServer();case 19:case"end":return e.stop()}}),e,null,[[5,15]])})))()},computed:{settings:function(){return M.state.settings},unsaved:function(){return M.state.unsaved},server:function(){return M.state.server},user:function(){return M.state.user},serverIcon:function(){var t=M.state.server;return t.icon?"https://cdn.discordapp.com/icons/".concat(t.id,"/").concat(t.icon,".png?size=128"):"https://cdn.discordapp.com/embed/avatars/1.png"}}},U=G,N=a("8212"),z=a("a523"),F=a("adda"),q=a("8270"),H=a("34c3"),J=a("e449"),K=a("8e36"),Y=a("2db4"),Q=a("2a7f"),W=Object(c["a"])(U,A,P,!1,null,null,null),X=W.exports;b()(W,{VApp:h["a"],VAppBar:_["a"],VAppBarNavIcon:x["a"],VAvatar:N["a"],VBtn:w["a"],VContainer:z["a"],VContent:V["a"],VDivider:C["a"],VIcon:y["a"],VImg:F["a"],VList:k["a"],VListItem:j["a"],VListItemAction:S["a"],VListItemAvatar:q["a"],VListItemContent:O["a"],VListItemIcon:H["a"],VListItemSubtitle:O["b"],VListItemTitle:O["c"],VMenu:J["a"],VNavigationDrawer:$["a"],VProgressLinear:K["a"],VSnackbar:Y["a"],VSpacer:I["a"],VToolbarTitle:Q["a"]});var Z=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("v-container",{staticClass:"px-3"},[n("v-alert",{attrs:{type:"info",text:""}},[t._v(" Bot & website are under development ")]),n("v-row",{staticClass:"my-5 my-sm-12",attrs:{align:"center","no-gutters":""}},[n("v-col",{staticClass:"px-4",attrs:{cols:"auto"}},[n("v-img",{attrs:{src:a("f900"),width:"150px"}})],1),n("v-col",{staticClass:"px-4"},[n("h1",{staticClass:"display-3 mb-3 mt-5 font-weight-bold",attrs:{id:"start"}},[t._v(" Lime ")]),n("h2",{staticClass:"display-1"},[t._v(" Keep calm. Lime is just there. ")]),n("v-btn",{staticClass:"mt-4 mr-3",attrs:{color:"primary",disabled:""}},[t._v(" Invite bot ")]),n("v-btn",{staticClass:"mt-4",attrs:{color:"secondary",to:"/manage"}},[t._v(" Dashboard ")])],1)],1),n("h1",{staticClass:"display-1 mb-3",staticStyle:{"margin-top":"100px"}},[t._v("Features")]),n("ul",{staticClass:"subtitle-1"},[n("li",[t._v("Very small functionality")]),n("li",[t._v("Beautiful panel with only one setting")]),n("li",[t._v("Fun commands")]),n("li",[t._v("More coming soon!")])])],1)},tt=[],et={name:"Home"},at=et,nt=a("0798"),rt=a("62ad"),it=a("0fd9"),st=Object(c["a"])(at,Z,tt,!1,null,null,null),ot=st.exports;b()(st,{VAlert:nt["a"],VBtn:w["a"],VCol:rt["a"],VContainer:z["a"],VImg:F["a"],VRow:it["a"]});var ct=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("v-container",[a("h1",{staticClass:"display-2 font-weight-bold mb-3 mt-5",attrs:{id:"start"}},[t._v(" Commands ")]),a("v-col",{attrs:{xl:"9"}},[t.loaded?a("v-expansion-panels",{model:{value:t.panels,callback:function(e){t.panels=e},expression:"panels"}},t._l(Object.keys(t.commands),(function(e){return a("v-expansion-panel",{key:e},[a("v-expansion-panel-header",{staticClass:"text-capitalize"},[t._v(t._s(e))]),a("v-expansion-panel-content",[a("v-list",[a("v-subheader",{staticClass:"pa-0"},[a("v-col",{attrs:{cols:"6",md:"4"}},[t._v("COMMAND")]),a("v-col",[t._v("DESCRIPTION")]),a("v-col",{staticClass:"d-none d-md-block",attrs:{cols:"2"}},[t._v("ALIASES")])],1),t._l(t.commands[e],(function(e,n){return a("v-list-item",{key:n,staticClass:"pa-0",on:{click:function(a){return t.open(e)}}},[a("v-list-item-content",[a("v-col",{attrs:{cols:"6",md:"4"}},[a("pre",{staticClass:"text-wrap"},[t._v(t._s(t.getUsage(e)))])]),a("v-col",[t._v(" "+t._s(e.description)+" ")]),a("v-col",{staticClass:"d-none d-md-block",attrs:{cols:"2"}},[a("pre",{staticClass:"text-wrap"},[t._v(t._s(e.aliases.join(", ")))])])],1)],1)}))],2)],1)],1)})),1):a("v-progress-linear",{attrs:{indeterminate:""}})],1),a("v-row",{attrs:{justify:"center"}},[a("v-dialog",{attrs:{"max-width":"400"},model:{value:t.dialog,callback:function(e){t.dialog=e},expression:"dialog"}},[a("v-card",[a("v-card-title",{staticClass:"headline text-capitalize"},[t._v(t._s(t.command.name))]),a("v-card-text",[a("p",[t._v(t._s(t.command.description))]),t.command.aliases.length>0?a("p",[a("b",[t._v("Aliases:")]),t._v(" "+t._s(t.command.aliases.join(", ")))]):t._e(),a("p",[a("b",[t._v("Usage:")]),a("pre",[t._v(t._s(t.getUsage(t.command)))])])]),a("v-card-actions",[a("v-spacer"),a("v-btn",{attrs:{text:"",color:"primary"},on:{click:function(e){t.dialog=!1}}},[t._v("Close")])],1)],1)],1)],1)],1)},lt=[],ut=(a("4160"),a("b0c0"),a("159b"),{data:function(){return{commands:null,panels:0,loaded:!1,dialog:!1,command:{name:"",description:"",overloads:"",aliases:[]}}},created:function(){var t=this;return Object(T["a"])(regeneratorRuntime.mark((function e(){return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:return e.next=2,t.$api.get("commands");case 2:t.commands=e.sent,t.loaded=!0;case 4:case"end":return e.stop()}}),e)})))()},methods:{getUsage:function(t){var e="!".concat(t.name);return!t.arguments||t.arguments.length<=0||t.arguments.forEach((function(t){t.optional?e+=" [".concat(t.name,"]"):e+=" <".concat(t.name,">")})),e},open:function(t){console.log(t),this.command=t,this.dialog=!0}}}),vt=ut,dt=a("b0af"),mt=a("99d9"),pt=a("169a"),ft=a("cd55"),gt=a("49e2"),bt=a("c865"),ht=a("0393"),_t=a("e0c7"),xt=Object(c["a"])(vt,ct,lt,!1,null,null,null),wt=xt.exports;b()(xt,{VBtn:w["a"],VCard:dt["a"],VCardActions:mt["a"],VCardText:mt["b"],VCardTitle:mt["c"],VCol:rt["a"],VContainer:z["a"],VDialog:pt["a"],VExpansionPanel:ft["a"],VExpansionPanelContent:gt["a"],VExpansionPanelHeader:bt["a"],VExpansionPanels:ht["a"],VList:k["a"],VListItem:j["a"],VListItemContent:O["a"],VProgressLinear:K["a"],VRow:it["a"],VSpacer:I["a"],VSubheader:_t["a"]});var Vt=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("v-container",[a("h1",{staticClass:"display-1"},[t._v("Manage "+t._s(t.server.name))]),a("div",{staticClass:"d-flex pa-3"},t._l(t.panels,(function(e){return a("v-card",{key:e.title,staticClass:"ma-3 pa-2",attrs:{width:"300",link:"",hover:"",to:e.url,append:""}},[a("v-card-text",[a("h2",{staticClass:"mb-3"},[t._v(" "+t._s(e.title)+" "),a("v-icon",{staticClass:"headline float-right"},[t._v(t._s(e.icon))])],1),a("div",[t._v(" "+t._s(e.description)+" ")])])],1)})),1)])},Ct=[],yt={data:function(){return{panels:[{title:"General settings",description:"Configure general options",url:"general",icon:"mdi-cogs"}],server:M.state.server}}},kt=yt,jt=Object(c["a"])(kt,Vt,Ct,!1,null,null,null),St=jt.exports;b()(jt,{VCard:dt["a"],VCardText:mt["b"],VContainer:z["a"],VIcon:y["a"]});var Ot=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("v-container",[a("h1",{staticClass:"display-1 mb-3"},[t._v("General settings")]),a("v-form",[a("v-row",[a("v-col",{attrs:{md:"3"}},[a("my-input",{attrs:{id:"prefix",label:"Prefix"}})],1)],1)],1)],1)},$t=[],It=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("v-text-field",{attrs:{id:t.id,label:t.label,required:"",width:"200px"},model:{value:t.value,callback:function(e){t.value=e},expression:"value"}})},Lt=[],Et=a("ade3"),At={props:{id:String,label:String},computed:{value:{get:function(){return M.state.settings[this.id]},set:function(t){M.commit("set",Object(Et["a"])({},this.id,t))}}}},Pt=At,Rt=a("8654"),Tt=Object(c["a"])(Pt,It,Lt,!1,null,null,null),Bt=Tt.exports;b()(Tt,{VTextField:Rt["a"]});var Dt={components:{MyInput:Bt}},Mt=Dt,Gt=a("4bd4"),Ut=Object(c["a"])(Mt,Ot,$t,!1,null,null,null),Nt=Ut.exports;b()(Ut,{VCol:rt["a"],VContainer:z["a"],VForm:Gt["a"],VRow:it["a"]});var zt=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("v-container",[a("h1",{staticClass:"display-1 mb-3"},[t._v("Select server")]),a("h3",[t._v("Logged in as "+t._s(this.user.username))]),a("div",{staticClass:"d-flex flex-wrap justify-center justify-sm-start pa-3"},t._l(this.user.guilds,(function(e){return a("router-link",{key:e.id,staticStyle:{"text-decoration":"none !important"},attrs:{to:t.getUrl(e)}},[a("v-tooltip",{attrs:{bottom:"",dark:""},scopedSlots:t._u([{key:"activator",fn:function(n){var r=n.on;return[a("v-img",t._g({staticClass:"ma-2 ma-sm-3 elevation-4",staticStyle:{"border-radius":"50%"},attrs:{src:t.getImg(e),gradient:t.getGradient(e),width:"70px",height:"70px"},scopedSlots:t._u([{key:"placeholder",fn:function(){return[a("v-row",{staticClass:"fill-height ma-0",attrs:{align:"center",justify:"center"}},[a("v-progress-circular",{attrs:{indeterminate:"",color:"grey lighten-5"}})],1)]},proxy:!0}],null,!0)},r),[e.botOnGuild?t._e():a("v-row",{staticClass:"fill-height m-0 title",attrs:{align:"center",justify:"center"}},[a("v-icon",[t._v("mdi-account-plus")])],1)],1)]}}],null,!0)},[a("span",[t._v(t._s(e.name))])])],1)})),1)])},Ft=[],qt={methods:{getImg:function(t){return t.icon?"https://cdn.discordapp.com/icons/".concat(t.id,"/").concat(t.icon,".png?size=128"):"https://cdn.discordapp.com/embed/avatars/1.png"},getGradient:function(t){return t.botOnGuild?null:"0deg, rgba(0,0,0,.75), rgba(0,0,0,.75)"},getUrl:function(t){return t.botOnGuild?"/manage/".concat(t.id):"/invite?id=".concat(t.id)}},computed:{user:function(){return M.state.user}}},Ht=qt,Jt=a("490a"),Kt=a("3a2f"),Yt=Object(c["a"])(Ht,zt,Ft,!1,null,null,null),Qt=Yt.exports;b()(Yt,{VContainer:z["a"],VIcon:y["a"],VImg:F["a"],VProgressCircular:Jt["a"],VRow:it["a"],VTooltip:Kt["a"]});var Wt=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("v-container",[a("h1",{staticClass:"mt-5 mb-3 display-3 font-weight-bold"},[t._v(" Error 404 ")]),a("h2",{staticClass:"display-1"},[t._v(" Page not found! ")])])},Xt=[],Zt={},te=Object(c["a"])(Zt,Wt,Xt,!1,null,null,null),ee=te.exports;b()(te,{VContainer:z["a"]}),n["default"].use(v["a"]);var ae=[{path:"/manage",component:X,children:[{path:"",component:Qt,meta:{nav:!1}},{path:":id",component:St},{path:":id/general",component:Nt}]},{path:"/logout",beforeEnter:function(t,e,a){n["default"].$cookies.remove("token"),a("/")}},{path:"/invite",beforeEnter:function(t){window.location.href=t.query.id?n["default"].$api.path("redirect/invite/?id=".concat(t.query.id)):n["default"].$api.path("invite")}},{path:"/callback",beforeEnter:function(t,e,a){t.query.token?(n["default"].$cookies.set("token",t.query.token),a("/manage")):a("/")}},{path:"/",component:E,children:[{path:"/",name:"Home",component:ot},{path:"/commands",name:"Commands",component:wt},{path:"*",component:ee}]}],ne=new v["a"]({mode:"history",base:"/",routes:ae}),re=ne,ie=a("ce5b"),se=a.n(ie),oe=(a("bf40"),a("8fa2"));n["default"].use(se.a);var ce=new se.a({preset:oe["preset"],theme:{dark:!0}}),le=a("2b27"),ue=a.n(le),ve=(a("d3b7"),"/api/"),de=function(){return{headers:{Authorization:n["default"].$cookies.get("token"),"Content-Type":"application/json"}}},me={get:function(t){return Object(T["a"])(regeneratorRuntime.mark((function e(){var a;return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:return e.next=2,fetch("".concat(ve).concat(t),de());case 2:return a=e.sent,e.next=5,a.json();case 5:return a=e.sent,e.abrupt("return",a);case 7:case"end":return e.stop()}}),e)})))()},put:function(t,e){return Object(T["a"])(regeneratorRuntime.mark((function a(){var n,r;return regeneratorRuntime.wrap((function(a){while(1)switch(a.prev=a.next){case 0:return n=de(),n.method="put",n.body=JSON.stringify(e),a.next=5,fetch("".concat(ve).concat(t),n);case 5:return r=a.sent,a.next=8,r.json();case 8:return r=a.sent,a.abrupt("return",r);case 10:case"end":return a.stop()}}),a)})))()},path:function(t){return"".concat(ve).concat(t)}};n["default"].config.productionTip=!1,n["default"].use(ue.a),n["default"].$cookies.config("7d"),n["default"].prototype.$api=n["default"].$api=me,new n["default"]({router:re,vuetify:ce,render:function(t){return t(u)}}).$mount("#app")},f900:function(t,e,a){t.exports=a.p+"img/lime.02a7a6eb.png"}});
//# sourceMappingURL=app.2a5fe8f4.js.map