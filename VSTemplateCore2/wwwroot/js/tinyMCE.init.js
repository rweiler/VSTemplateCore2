tinymce.init({
    // Core options
    selector: "textarea.mceEditor",
    theme: "modern",
    fix_list_elements: true,
    body_class: "mceEditorContainer",
    relative_urls: false,
    remove_script_host: true,
    branding: false,

    // Plugin Options
    image_advtab: true,
    plugins: "advlist anchor autolink autosave charmap code emoticons fullscreen hr image media link lists nonbreaking paste preview searchreplace table textcolor visualblocks visualchars wordcount moxiemanager",
    toolbar1: "bold italic underline strikethrough | superscript subscript | charmap | alignlieft aligncenter alignright alignjustify blockquote | undo redo removeformat | preview | code",
    toolbar2: "styleselect fontsizeselect forecolor | bullist numlist outdent indent | link unlink anchor | image media | table",
    fontsize_formats: "0.7rem 0.8rem 1.0rem 1.2rem 1.5rem 2.0rem 3.0rem",
    font_formats: "Andale Mono=andale mono,times;" +
    "Arial=arial,helvetica,sans-serif;" +
    "Arial Black=arial black,avant garde;" +
    "Book Antiqua=book antiqua,palatino;" +
    "Comic Sans MS=comic sans ms,sans-serif;" +
    "Courier New=courier new,courier;" +
    "Georgia=georgia,palatino;" +
    "Tahoma=tahoma,arial,helvetica,sans-serif;" +
    "Impact=impact,chicago;" +
    "Times New Roman=times new roman,times;" +
    "Trebuchet MS=trebuchet ms,geneva;" +
    "Verdana=verdana,geneva",
    style_formats_autohide: true,
    style_formats_merge: true,
    style_formats: [
        { title: 'Table: Cell Align Top', selector: 'td', styles: { 'vertical-align': 'top' } },
        { title: 'Table: Cell Align Bottom', selector: 'td', styles: { 'vertical-align': 'bottom' } },
        { title: 'Table: Cell Border Top', selector: 'td', styles: { 'border-top': '1px solid #77787b' } },
        { title: 'Table: Cell Border Bottom', selector: 'td', styles: { 'border-bottom': '1px solid #77787b' } },
        { title: 'Responsive Image', selector: 'img', classes: 'img-responsive' },
        { title: 'Responsive Video <div>', selector: 'div', classes: 'embed-responsive embed-responsive-16by9' },
        { title: 'Responsive Video <ifrmae>', selector: 'iframe', classes: 'embed-responsive-item' }
    ]
});