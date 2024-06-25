import { createApp } from 'vue'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import router from './router/router'
import main from './components/MyMain.vue'

import zhCn from 'element-plus/dist/locale/zh-cn.mjs'  //el日历设置中文地区语言

const debounce = (fn, delay) => {
    let timer = null;
    return function () {
      let context = this;
      let args = arguments;
      clearTimeout(timer);
      timer = setTimeout(function () {
        fn.apply(context, args);
      }, delay);
    }
  }
  
  const _ResizeObserver = window.ResizeObserver;
  window.ResizeObserver = class ResizeObserver extends _ResizeObserver {
    constructor(callback) {
      callback = debounce(callback, 16);
      super(callback);
    }
  } //2023.9.3 上面两个函数 解决页面缩小时el-table引起ResizeObserver报错问题
 
createApp(main).use(ElementPlus,{locale: zhCn}).use(router).mount('#app')
