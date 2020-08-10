import { AuthGuard } from './infrastructure/services/auth-guard.service';
export var routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', loadChildren: './home/home.module#HomeModule' },
    { path: 'prescription', loadChildren: './prescription/prescription.module#PrescriptionModule', canActivate: [AuthGuard] },
    { path: 'setting', loadChildren: './setting/setting.module#SettingModule', canActivate: [AuthGuard] },
    { path: 'patient', loadChildren: './patient/patient.module#PatientModule', canActivate: [AuthGuard] },
    { path: 'status', loadChildren: './status/status.module#StatusModule' },
    { path: '**', redirectTo: '/status/404' }
];
//# sourceMappingURL=app.routes.js.map