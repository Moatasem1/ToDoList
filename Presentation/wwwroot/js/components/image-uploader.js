export const ImageUploader = {
  template: `
    <div class="mb-3 d-flex flex-column flex-grow-1">
      <label class="form-label d-block">Image</label>

      <div class="flex-grow-1 position-relative border rounded d-flex justify-content-center align-items-center  p-3 overflow-hidden bg-white"
           @click="triggerFileInput">
        
        <img v-if="preview" :src="preview" class="position-absolute img-fluid h-100 object-fit-contain" alt="Preview" />

        <span v-else class="text-muted">Click to select an image</span>

        <button v-if="preview"
                type="button"
                @click.stop="removeImage"
                class="btn btn-sm btn-danger position-absolute top-0 end-0 m-2"
                title="Remove image">
            Remove
        </button>
      </div>

      <input
        ref="fileInput"
        type="file"
        accept="image/*"
        class="form-control mt-2"
        @change="onFileChange"
        style="display: none;"
      />
    </div>
  `,
  props: {
    imagePreview: String
  },
  emits: ['imageChange'],
  data() {
    return {
      preview: this.imagePreview || null
    };
  },
  watch: {
    imagePreview(newVal) {
      this.preview = newVal;
    }
  },
  methods: {
    onFileChange(event) {
      const file = event.target.files[0];
      if (!file) {
        this.removeImage();
        return;
      }

      const reader = new FileReader();
      reader.onload = () => {
        this.preview = reader.result;
        this.$emit('imageChange', reader.result);
      };
      reader.readAsDataURL(file);
    },
    triggerFileInput() {
      this.$refs.fileInput.click();
    },
    removeImage() {
      this.preview = null;
      this.$refs.fileInput.value = null; // Clear input
      this.$emit('imageChange', null);
    }
  }
};
