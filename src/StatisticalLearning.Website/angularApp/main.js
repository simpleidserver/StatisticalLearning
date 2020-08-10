import './styles.scss';
import 'zone.js';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import 'ol/ol.css';
import { AppModule } from './app/app.module';
if (module.hot) {
    module.hot.accept();
}
platformBrowserDynamic().bootstrapModule(AppModule);
//# sourceMappingURL=main.js.map