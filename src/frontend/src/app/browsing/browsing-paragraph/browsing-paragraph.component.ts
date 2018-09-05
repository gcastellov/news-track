import { Component } from '@angular/core';
import { BrowsingItemComponent } from '../browsing-item/browsing-item.component';
import { BrowsingElement } from '../browsing-element';

@Component({
    selector: 'app-browsing-paragraph',
    templateUrl: './browsing-paragraph.component.html',
    styleUrls: ['./browsing-paragraph.component.less']
})
export class BrowsingParagraphComponent extends BrowsingItemComponent {

    onChange(entry: BrowsingElement): void {
        if (!entry.isSelected) {
            const index = this.model.paragraphs.findIndex(p => p === entry.content);
            if (index >= 0) {
                this.model.paragraphs.splice(index, 1);
            }
        } else {
            this.model.paragraphs.push(entry.content);
        }
    }
}
