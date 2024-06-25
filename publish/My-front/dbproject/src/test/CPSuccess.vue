<template>
  <div class="success-page">
    <h1>密码修改成功</h1>
    <p>您的密码已成功修改。</p>
    <button @click="goToLogin">返回登录</button>
  </div>
</template>

<script>
export default {
  methods: {
    goToLogin() {
      this.$router.push('/login');
    }
  }
};
</script>

<style scoped>
.success-page {
  text-align: center;
  padding: 20px;
}
</style>
