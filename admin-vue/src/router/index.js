import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import AdminLayout from '../layouts/AdminLayout.vue'

const routes = [
  {
    path: '/login',
    name: 'Login',
    component: () => import('../views/Login.vue'),
    meta: { public: true }
  },
  {
    path: '/',
    component: AdminLayout,
    meta: { requiresAuth: true },
    children: [
      { path: '', redirect: '/dashboard' },
      { path: 'dashboard', name: 'Dashboard', component: () => import('../views/Dashboard.vue') },
      { path: 'dashboard/overview', name: 'DashboardOverview', component: () => import('../views/Dashboard.vue') },
      { path: 'dashboard/daily', name: 'DashboardDaily', component: () => import('../views/Dashboard.vue') },
      { path: 'cranes', name: 'Cranes', component: () => import('../views/cranes/CraneList.vue') },
      { path: 'cranes/new', name: 'CranesNew', component: () => import('../views/cranes/CraneForm.vue') },
      { path: 'cranes/edit/:id', name: 'CranesEdit', component: () => import('../views/cranes/CraneForm.vue') },
      { path: 'cranes/maintenance', name: 'CranesMaintenance', component: () => import('../views/Placeholder.vue') },
      { path: 'cranes/history', name: 'CranesHistory', component: () => import('../views/Placeholder.vue') },
      { path: 'operations/rentals', name: 'OperationsRentals', component: () => import('../views/Placeholder.vue') },
      { path: 'operations/rentals/new', name: 'OperationsRentalsNew', component: () => import('../views/Placeholder.vue') },
      { path: 'work-plans', name: 'WorkPlans', component: () => import('../views/workplans/WorkPlanList.vue') },
      { path: 'work-plans/new', name: 'WorkPlansNew', component: () => import('../views/workplans/WorkPlanForm.vue') },
      { path: 'work-plans/edit/:id', name: 'WorkPlansEdit', component: () => import('../views/workplans/WorkPlanForm.vue') },
      { path: 'operations/tracking', name: 'OperationsTracking', component: () => import('../views/Placeholder.vue') },
      { path: 'companies', name: 'Companies', component: () => import('../views/companies/CompanyList.vue') },
      { path: 'companies/new', name: 'CompaniesNew', component: () => import('../views/companies/CompanyForm.vue') },
      { path: 'companies/edit/:id', name: 'CompaniesEdit', component: () => import('../views/companies/CompanyForm.vue') },
      { path: 'companies/projects', name: 'CompaniesProjects', component: () => import('../views/Placeholder.vue') },
      { path: 'quotes', name: 'Quotes', component: () => import('../views/Placeholder.vue') },
      { path: 'quotes/new', name: 'QuotesNew', component: () => import('../views/Placeholder.vue') },
      { path: 'quotes/approvals', name: 'QuotesApprovals', component: () => import('../views/Placeholder.vue') },
      { path: 'finance/invoices', name: 'FinanceInvoices', component: () => import('../views/Placeholder.vue') },
      { path: 'income-expense', name: 'IncomeExpense', component: () => import('../views/income-expense/IncomeExpenseList.vue') },
      { path: 'income-expense/new', name: 'IncomeExpenseNew', component: () => import('../views/income-expense/IncomeExpenseForm.vue') },
      { path: 'income-expense/edit/:id', name: 'IncomeExpenseEdit', component: () => import('../views/income-expense/IncomeExpenseForm.vue') },
      { path: 'maintenance/plan', name: 'MaintenancePlan', component: () => import('../views/maintenance/MaintenanceList.vue') },
      { path: 'maintenance/records', name: 'MaintenanceRecords', component: () => import('../views/maintenance/MaintenanceList.vue') },
      { path: 'maintenance/records/new', name: 'MaintenanceRecordsNew', component: () => import('../views/maintenance/MaintenanceForm.vue') },
      { path: 'maintenance/records/edit/:id', name: 'MaintenanceRecordsEdit', component: () => import('../views/maintenance/MaintenanceForm.vue') },
      { path: 'maintenance/history', name: 'MaintenanceHistory', component: () => import('../views/maintenance/MaintenanceList.vue') },
      { path: 'operators', name: 'Operators', component: () => import('../views/operators/OperatorList.vue') },
      { path: 'operators/new', name: 'OperatorsNew', component: () => import('../views/Placeholder.vue') },
      { path: 'operators/assignments', name: 'OperatorsAssignments', component: () => import('../views/operators/OperatorAssignments.vue') },
      { path: 'reports/crane-usage', name: 'ReportsCraneUsage', component: () => import('../views/Placeholder.vue') },
      { path: 'reports/rentals', name: 'ReportsRentals', component: () => import('../views/Placeholder.vue') },
      { path: 'reports/income', name: 'ReportsIncome', component: () => import('../views/Placeholder.vue') },
      { path: 'reports/operator', name: 'ReportsOperator', component: () => import('../views/Placeholder.vue') },
      { path: 'cms/sliders', name: 'CmsSliders', component: () => import('../views/Placeholder.vue') },
      { path: 'cms/news', name: 'CmsNews', component: () => import('../views/Placeholder.vue') },
      { path: 'cms/gallery', name: 'CmsGallery', component: () => import('../views/Placeholder.vue') },
      { path: 'cms/services', name: 'CmsServices', component: () => import('../views/Placeholder.vue') },
      { path: 'cms/references', name: 'CmsReferences', component: () => import('../views/Placeholder.vue') },
      { path: 'cms/pages', name: 'CmsPages', component: () => import('../views/Placeholder.vue') },
      { path: 'cms/contact', name: 'CmsContact', component: () => import('../views/Placeholder.vue') },
      { path: 'users', name: 'Users', component: () => import('../views/users/UserList.vue') },
      { path: 'system/roles', name: 'SystemRoles', component: () => import('../views/Placeholder.vue') },
      { path: 'system/permissions', name: 'SystemPermissions', component: () => import('../views/Placeholder.vue') },
      { path: 'system/menus', name: 'SystemMenus', component: () => import('../views/system/MenuManage.vue') },
      { path: 'system/logs', name: 'SystemLogs', component: () => import('../views/Placeholder.vue') },
      { path: 'settings/general', name: 'SettingsGeneral', component: () => import('../views/Placeholder.vue') },
      { path: 'settings/sms', name: 'SettingsSms', component: () => import('../views/Placeholder.vue') },
      { path: 'settings/email', name: 'SettingsEmail', component: () => import('../views/Placeholder.vue') },
      { path: 'settings/api', name: 'SettingsApi', component: () => import('../views/Placeholder.vue') }
    ]
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach(async (to, _from, next) => {
  const auth = useAuthStore()
  auth.initFromStorage()
  if (to.meta.public) {
    if (auth.isAuthenticated) return next('/dashboard')
    return next()
  }
  if (to.meta.requiresAuth && !auth.isAuthenticated) return next('/login')
  next()
})

export default router
