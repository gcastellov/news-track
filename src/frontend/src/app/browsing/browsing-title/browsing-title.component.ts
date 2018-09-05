import { Component } from '@angular/core';
import { BrowsingItemComponent } from '../browsing-item/browsing-item.component';

@Component({
    selector: 'app-browsing-title',
    templateUrl: './browsing-title.component.html',
    styleUrls: ['./browsing-title.component.less']
})
export class BrowsingTitleComponent extends BrowsingItemComponent {
}
