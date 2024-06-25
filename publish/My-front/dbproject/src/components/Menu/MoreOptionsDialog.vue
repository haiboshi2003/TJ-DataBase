<template>
  <div style="min-width:600px">
    <!-- 其他内容 -->
    <el-dialog title="更多功能" v-model="dialogVisible" @open="dialogVisible" @close="handleClose">
      <h2>警员管理<br><div style="color:#d7caaa">----------------------------------------------------------------</div></h2>
      <el-checkbox-group :modelValue="internalSelectedOptions" @update:modelValue="internalSelectedOptions = $event">
        <el-checkbox style="width:150px" id="cb" v-for="option in options" :key="option.label" :label="option.label" :disabled="option.authority > myAuthority">{{ option.label }}</el-checkbox>
      </el-checkbox-group>

      <h2><br>案件管理<br><div style="color:#d7caaa">----------------------------------------------------------------</div></h2>
      <el-checkbox-group :modelValue="internalSelectedOptions" @update:modelValue="internalSelectedOptions = $event">
        <el-checkbox style="width:150px" id="cb" v-for="option in options2" :key="option.label" :label="option.label" :disabled="option.authority > myAuthority">{{ option.label }}</el-checkbox>
      </el-checkbox-group>
      
      <h2><br>装备管理<br><div style="color:#d7caaa">----------------------------------------------------------------</div></h2>
      <el-checkbox-group :modelValue="internalSelectedOptions" @update:modelValue="internalSelectedOptions = $event">
        <el-checkbox style="width:150px" id="cb" v-for="option in options3" :key="option.label" :label="option.label" :disabled="option.authority > myAuthority">{{ option.label }}</el-checkbox>
      </el-checkbox-group>

      <h2><br>城区和居民管理<br><div style="color:#d7caaa">----------------------------------------------------------------</div></h2>
      <el-checkbox-group :modelValue="internalSelectedOptions" @update:modelValue="internalSelectedOptions = $event">
        <el-checkbox style="width:150px" id="cb" v-for="option in options4" :key="option.label" :label="option.label" :disabled="option.authority > myAuthority">{{ option.label }}</el-checkbox>
      </el-checkbox-group>

      <!-- <h2><br>数据质量管理<br><div style="color:#d7caaa">----------------------------------------------------------------</div></h2>
      <el-checkbox-group :modelValue="internalSelectedOptions" @update:modelValue="internalSelectedOptions = $event">
        <el-checkbox style="width:150px" id="cb" v-for="option in options5" :key="option.label" :label="option.label" :disabled="option.authority > myAuthority">{{ option.label }}</el-checkbox>
      </el-checkbox-group> -->

      <h2><br>案件办理<br><div style="color:#d7caaa">----------------------------------------------------------------</div></h2>
      <el-checkbox-group :modelValue="internalSelectedOptions" @update:modelValue="internalSelectedOptions = $event">
        <el-checkbox style="width:150px" id="cb" v-for="option in options6" :key="option.label" :label="option.label" :disabled="option.authority > myAuthority">{{ option.label }}</el-checkbox>
      </el-checkbox-group>
      <!-- <h2><br>系统日志管理<br><div style="color:#d7caaa">----------------------------------------------------------------</div></h2>
      <el-checkbox-group :modelValue="internalSelectedOptions" @update:modelValue="internalSelectedOptions = $event">
        <el-checkbox style="width:150px" id="cb" v-for="option in options7" :key="option.label" :label="option.label" :disabled="option.authority > myAuthority">{{ option.label }}</el-checkbox>
      </el-checkbox-group> -->
      <el-button @click="confirmOptions">确认</el-button>
    </el-dialog>

    <el-dialog title="警告" v-model="errdialogVisible" @close="errClose"><h2>最多只能选择12个功能！</h2>
      <el-button @click="errConfirm">确认</el-button>
    </el-dialog>
  </div>
</template>

<script>
export default {

  data() {
    return {
      dialogVisible: false,
      errdialogVisible: false,
      internalSelectedOptions: [], // 修改此处使用一个内部属性
      myAuthority: 0,
      options: [
        { label: '注册', authority: '1', icon: 'el-icon-star-off' },
        { label: '警员权限修改', authority: '2', icon: 'el-icon-user' },
        { label: '警员考核管理', authority: '1', icon: 'el-icon-suitcase' },
        { label: '警局信息管理', authority: '4', icon: 'el-icon-date' },
        { label: '警员信息管理', authority: '0', icon: 'el-icon-food' },
        { label: '警员出勤管理', authority: '1', icon: 'el-icon-food' },
        { label: '执法录像管理', authority: '1', icon: 'el-icon-food' },
        { label: '接警记录管理', authority: '1', icon: 'el-icon-food' },
        { label: '薪水管理', authority: '4', icon: 'el-icon-food' },
        // 其他选项
      ],
      options2: [
        // { label: '证据收纳', authority: '1', icon: 'el-icon-star-off' },
        { label: '嫌疑人信息管理', authority: '1', icon: 'el-icon-star-off' },
        { label: '犯罪记录管理', authority: '1', icon: 'el-icon-star-off' },
        { label: '受害人信息管理', authority: '1', icon: 'el-icon-star-off' },
        { label: '案件分类与管理', authority: '1', icon: 'el-icon-star-off' },
        { label: '犯罪数据统计', authority: '1', icon: 'el-icon-star-off' },
        { label: '重点人员统计', authority: '1', icon: 'el-icon-star-off' },
        // 其他选项
      ],
      options3:[
        { label: '警车管理', authority: '1', icon: 'el-icon-star-off' },
        { label: '警械管理', authority: '1', icon: 'el-icon-star-off' },
      ],
      options4:[
        { label: '公民户籍管理', authority: '1', icon: 'el-icon-star-off' },
        { label: '城市地区管理', authority: '1', icon: 'el-icon-star-off' },
        { label: '城市调度管理', authority: '1', icon: 'el-icon-star-off' },
        { label: '家族背景调查', authority: '1', icon: 'el-icon-star-off' },
      ],
      options5:[
        { label: '数据质量管理', authority: '1', icon: 'el-icon-star-off' },
      ],
      options6:[
        { label: '案件办理', authority: '1', icon: 'el-icon-star-off' },
      ],
      options7:[
        { label: '系统日志管理', authority: '1', icon: 'el-icon-star-off' },
      ],
    };
  },
  props: {
    selectedOptions: {
      type: Array,
      default: () => []
    }
  },
  methods: {
    showMoreOptions(selectedOptions, authority) {
      console.log('打开更多功能');
      this.internalSelectedOptions = selectedOptions; // 修改此处为内部属性
      this.myAuthority=authority;
      console.log(this.myAuthority);
      this.dialogVisible = true;
      console.log(this.internalSelectedOptions);
    },
    handleClose() {
      this.dialogVisible = false;
    },
    confirmOptions() {
      // 在这里处理选项确认的逻辑
      // 更新常用功能栏的显示等
      var cnt = this.internalSelectedOptions.length;
      console.log(cnt);
      if(cnt <= 12){
        this.dialogVisible = false;
        this.$emit('options-selected', this.internalSelectedOptions); // 发送事件到父组件，传递选中的选项
      }
      else{
        this.errdialogVisible = true;
      }
    },
    errClose() {
      this.errdialogVisible = false;
    },
    errConfirm(){
      this.errdialogVisible = false;
    }
  },
}
</script>