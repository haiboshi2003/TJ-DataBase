<template>
  <el-header class="sub-header" @mousemove="handleMouseMove">
    <div>&nbsp;&nbsp;城区和居民管理&nbsp;>&nbsp;公民户籍管理</div>
  </el-header>

  <el-tabs v-model="anv" type="border-card" style="margin-top: 10vh;">
    <!-- 查询栏 -->
    <el-tab-pane label="查询公民信息" name="1">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">身份证号:</el-text>
              <el-input class="inputBox" v-model="citizenID" placeholder="请输入身份证号" clearable maxlength="18"
                oninput="value=value.replace(/[^\d.]/g,'')" style="width: 235px;" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">姓名:</el-text>
              <el-input class="inputBox" v-model="citizenName" placeholder="请输入姓名" clearable />
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">性别:</el-text>
              <el-select class="inputBox" v-model="citizenSex" style="width: 235px;" placeholder="请选择">
                <el-option label="全部性别" selected value=""></el-option>
                <el-option label="男" value="M"></el-option>
                <el-option label="女" value="F"></el-option>
              </el-select>
            </td>
            <td>
              <el-text class="noteText" type="primary">住址:</el-text>
              <el-input class="inputBox" v-model="citizenAddress" placeholder="请输入地址" clearable />
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="fetchCitizenInfo">查询</el-button>
        </div>
      </div>

      <el-divider />

      <div>
        <!-- 表格显示获取的公民信息 -->
        <el-table :data="citizenInfo" stripe height="450" @wheel.passive.stop>
          <el-table-column prop="id" label="身份证号" sortable />
          <el-table-column prop="name" label="公民名称" sortable width="120px" />
          <el-table-column prop="sex" label="性别" sortable width="80px" />
          <el-table-column prop="address" label="住址" sortable />
          <el-table-column prop="motherid" label="母亲身份证号" sortable />
          <el-table-column prop="fatherid" label="父亲身份证号" sortable />
        </el-table>
      </div>
    </el-tab-pane>

    <!-- 新增栏 -->
    <el-tab-pane label="公民信息登记" name="2">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">身份证号:</el-text>
              <el-input class="inputBox" v-model="addCitizenID" placeholder="请输入身份证号" clearable maxlength="18"
                oninput="value=value.replace(/[^\d.]/g,'')" style="width: 235px;" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">姓名:</el-text>
              <el-input class="inputBox" v-model="addCitizenName" placeholder="请输入姓名" clearable />
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">母亲身份证号:</el-text>
              <el-input class="inputBox" v-model="addMotherCitizenID" placeholder="请输入身份证号" clearable maxlength="18"
                oninput="value=value.replace(/[^\d.]/g,'')" style="width: 235px;" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">住址:</el-text>
              <el-input class="inputBox" v-model="addCitizenAddress" placeholder="请输入地址" clearable />
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">父亲身份证号:</el-text>
              <el-input class="inputBox" v-model="addFatherCitizenID" placeholder="请输入身份证号" clearable maxlength="18"
                oninput="value=value.replace(/[^\d.]/g,'')" style="width: 235px;" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">性别:</el-text>
              <el-select class="inputBox" v-model="addCitizenSex" placeholder="请选择">
                <el-option label="男" value="M"></el-option>
                <el-option label="女" value="F"></el-option>
              </el-select>
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="addCitizenInfo">上传公民信息</el-button>
        </div>
      </div>
    </el-tab-pane>

    <!-- 删除栏 -->
    <el-tab-pane label="删除公民信息" name="3">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">身份证号：</el-text>
              <el-input class="inputBox" v-model="delCitizenID" placeholder="请输入身份证号" clearable maxlength="18"
                oninput="value=value.replace(/[^\d.]/g,'')" style="width: 250px;" show-word-limit />
            </td>
            <td>
              <el-button type="primary" @click="delCitizenInfo" style="margin-left: 20px">删除公民信息</el-button>
            </td>
          </tr>
        </table>
      </div>
    </el-tab-pane>

    <!-- 修改栏 -->
    <el-tab-pane label="修改公民信息" name="4">
      <div style="text-align: center;">
        <table style="display: inline-block;text-align: left;">
          <tr>
            <td>
              <el-text class="noteText" type="primary">身份证号:</el-text>
              <el-input class="inputBox" v-model="updCitizenID" placeholder="请输入身份证号" clearable maxlength="18"
                oninput="value=value.replace(/[^\d.]/g,'')" style="width: 235px;" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">更新姓名:</el-text>
              <el-input class="inputBox" v-model="updCitizenName" placeholder="请输入姓名" clearable />
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">更新母亲身份证号:</el-text>
              <el-input class="inputBox" v-model="updMotherCitizenID" placeholder="请输入身份证号" clearable maxlength="18"
                oninput="value=value.replace(/[^\d.]/g,'')" style="width: 235px;" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">更新住址:</el-text>
              <el-input class="inputBox" v-model="updCitizenAddress" placeholder="请输入地址" clearable />
            </td>
          </tr>
          <tr>
            <td>
              <el-text class="noteText" type="primary">更新父亲身份证号:</el-text>
              <el-input class="inputBox" v-model="updFatherCitizenID" placeholder="请输入身份证号" clearable maxlength="18"
                oninput="value=value.replace(/[^\d.]/g,'')" style="width: 235px;" show-word-limit />
            </td>
            <td>
              <el-text class="noteText" type="primary">更新性别:</el-text>
              <el-select class="inputBox" v-model="updCitizenSex" placeholder="请选择">
                <el-option label="男" value="M"></el-option>
                <el-option label="女" value="F"></el-option>
              </el-select>
            </td>
          </tr>
        </table>

        <div class="seaButton">
          <el-button type="primary" @click="updCitizenInfo">更新公民信息</el-button>
        </div>
      </div>
    </el-tab-pane>
  </el-tabs>
</template>

<script>
import axios from "../../api/request";

import { ElMessage } from 'element-plus';

export default {
  data() {
    return {
      anv: "1",
      citizenID: '',
      citizenName: '',
      citizenAddress: '',
      citizenSex: "",
      citizenInfo: [],
      addCitizenID: "",
      addCitizenName: "",
      addCitizenAddress: "",
      addCitizenSex: "",
      addMotherCitizenID: "",
      addFatherCitizenID: "",
      delCitizenID: "",
      updCitizenID: "",
      updCitizenName: "",
      updCitizenAddress: "",
      updCitizenSex: "",
      updMotherCitizenID: "",
      updFatherCitizenID: "",
    }
  },
  methods: {
    handleMouseMove(event) {
      const x = event.pageX - event.target.offsetLeft;
      const y = event.pageY - event.target.offsetTop;

      event.target.style.setProperty("--x", `${x}px`);
      event.target.style.setProperty("--y", `${y}px`);
    },
    async fetchCitizenInfo() {
      try {
        const response = await axios.get(`/api/getCitizenInfo?citizenID=${encodeURIComponent(this.citizenID)}&citizenName=${encodeURIComponent(this.citizenName)}&citizenSex=${encodeURIComponent(this.citizenSex)}&citizenAddress=${encodeURIComponent(this.citizenAddress)}`);
        this.citizenInfo = response.data.map(item => {
          if (item.sex === 'M') {
            return { ...item, sex: '男' };
          } else if (item.sex === 'F') {
            return { ...item, sex: '女' };
          } else {
            return item;
          }
        });
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "获取公民列表失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async addCitizenInfo() {
      try {
        const response = await axios.post(`/api/addCitizen?citizenID=${encodeURIComponent(this.addCitizenID)}&citizenName=${encodeURIComponent(this.addCitizenName)}&citizenSex=${encodeURIComponent(this.addCitizenSex)}&citizenAddress=${encodeURIComponent(this.addCitizenAddress)}&motherID=${encodeURIComponent(this.addMotherCitizenID)}&fatherID=${encodeURIComponent(this.addFatherCitizenID)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchCitizenInfo();
          ElMessage({
            showClose: true,
            message: '新增公民成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "新增公民失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async delCitizenInfo() {
      try {
        const response = await axios.delete(`/api/delCitizen?citizenID=${encodeURIComponent(this.delCitizenID)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchCitizenInfo();
          ElMessage({
            showClose: true,
            message: '删除公民成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "删除公民失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async updCitizenInfo() {
      try {
        const response = await axios.put(`/api/updCitizen?citizenID=${encodeURIComponent(this.updCitizenID)}&citizenName=${encodeURIComponent(this.updCitizenName)}&citizenSex=${encodeURIComponent(this.updCitizenSex)}&citizenAddress=${encodeURIComponent(this.updCitizenAddress)}&motherID=${encodeURIComponent(this.updMotherCitizenID)}&fatherID=${encodeURIComponent(this.updFatherCitizenID)}`);
        if (typeof response.data == "string") {
          ElMessage({
            showClose: true,
            message: response.data,
            type: 'warning',
            duration: 5000,
          });
        }
        else {
          await this.fetchCitizenInfo();
          ElMessage({
            showClose: true,
            message: '修改成功! ',
            type: 'success',
            duration: 5000,
          });
        }
      } catch (error) {
        // 请求失败时的处理逻辑
        ElMessage({
          showClose: true,
          message: "修改失败！",
          type: 'error',
          duration: 5000,
        });
      }
    },
    async back() {
      this.citizenUseInfo = "";
      this.showCitizen = true;
    },
  },
  async created() {
    await this.fetchCitizenInfo();
  },
};
</script>

<style lang="postcss" scoped>
.sub-header {
  overflow: hidden;
  display: flex;
  position: absolute;
  top: 70px;
  left: 199px;
  width: calc(100% - 199px);
  height: 7vh;
  min-height: 40px;
  align-items: center;
  /* 文字竖直方向居中对齐 */
  background-color: #1f2cdf;
  box-shadow: inset -500px 0px 200px 0px rgba(4, 0, 113, 0.856);
  color: #ffffff;
  font-size: 28px;
}

.sub-header::before {
  --size: 0;
  content: '';
  position: absolute;
  left: var(--x);
  top: var(--y);
  width: var(--size);
  height: var(--size);
  background: radial-gradient(circle closest-side, #5a65ff, transparent);
  transform: translate(-50%, -50%);
  transition: width .2s ease, height .2s ease;
}

.sub-header:hover::before {
  --size: 400px;
}

.noteText {
  font-size: 18px;
  text-align: center;
  margin: 20px 0px;
  width: 12vw;
  display: inline-block;
}

.inputBox {
  display: inline-block;
  width: 200px;
  height: 32px;
  margin: 20px 10px;
}

.seaButton {
  display: block;
  text-align: center;
  margin-top: 15px;
  margin-bottom: 10px;
}
</style>